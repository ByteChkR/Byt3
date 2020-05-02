﻿using System.Collections.Generic;
using System.Linq;
using Byt3.ADL;
using Byt3.OpenCL.DataTypes;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers;

namespace Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects
{
    public class FLProgram : ALoggable<LogType>
    {
        public static IDebugger Debugger = null;

        private readonly Stack<FLExecutionContext> ContextStack = new Stack<FLExecutionContext>();
        private readonly List<FLBuffer> internalBuffers = new List<FLBuffer>();
        private MemoryBuffer activeChannelBuffer;
        private byte[] activeChannels;
        private bool channelDirty = true;


        public FLProgram(Dictionary<string, ExternalFlFunction> definedScripts,
            Dictionary<string, FLBuffer> definedBuffers, Dictionary<string, FLFunction> flFunctions) : base(OpenFLDebugConfig.Settings)
        {
            FlFunctions = flFunctions;
            DefinedBuffers = definedBuffers;
            DefinedScripts = definedScripts;
            InternalState = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, FLBuffer> definedBuffer in DefinedBuffers)
            {
                InternalState.Add(definedBuffer.Key, true);
            }
        }

        //Get set when calling Run/SetCLVariables
        public CLAPI Instance { get; private set; }
        internal FLBuffer ActiveBuffer { get; set; }

        public byte[] ActiveChannels
        {
            get => activeChannels;
            internal set
            {
                if (activeChannels == null || channelDirty)
                {
                    channelDirty = true;
                }
                else
                {
                    for (int i = 0; i < activeChannels.Length; i++)
                    {
                        if (value[i] != activeChannels[i])
                        {
                            channelDirty = true;
                            break;
                        }
                    }

                    if (!channelDirty)
                    {
                        return;
                    }
                }

                activeChannels = value;
            }
        }

        internal MemoryBuffer ActiveChannelBuffer
        {
            get
            {
                if (channelDirty)
                {
                    if (activeChannelBuffer == null)
                    {
                        activeChannelBuffer = CLAPI.CreateBuffer(Instance, activeChannels, MemoryFlag.WriteOnly,
                            "ActiveChannelBuffer");
                    }
                    else if (activeChannelBuffer.IsDisposed)
                    {
                        activeChannelBuffer = CLAPI.CreateBuffer(Instance, activeChannels, MemoryFlag.WriteOnly,
                            "ActiveChannelBuffer");
                    }
                    else
                    {
                        CLAPI.WriteToBuffer(Instance, activeChannelBuffer, activeChannels);
                    }

                    channelDirty = false;
                }

                return activeChannelBuffer;
            }
        }

        internal FLBuffer Input { get; set; }
        public int3 Dimensions => new int3(Input.Width, Input.Height, 4);
        public int InputSize => Dimensions.x * Dimensions.y * Dimensions.z;

        private Dictionary<string, bool> InternalState { get; }
        public Dictionary<string, FLBuffer> DefinedBuffers { get; }
        public string[] BufferNames => DefinedBuffers.Keys.ToArray();
        public Dictionary<string, FLFunction> FlFunctions { get; }
        public Dictionary<string, ExternalFlFunction> DefinedScripts { get; }

        public bool HasBufferWithName(string name)
        {
            return DefinedBuffers.ContainsKey(name);
        }

        public FLBuffer GetBufferWithName(string name, bool makeUnmanaged)
        {
            FLBuffer ret = DefinedBuffers[name];
            if (makeUnmanaged)
            {
                InternalState[name] = false;
            }

            return ret;
        }

        internal FLFunction EntryPoint => FlFunctions.First(x => x.Key == "Main").Value;

        public FLBuffer GetActiveBuffer(bool makeUnmanaged)
        {
            FLBuffer ret = ActiveBuffer;
            if (makeUnmanaged)
            {
                InternalState[ret.DefinedBufferName] = false;
            }

            return ret;
        }

        internal void RemoveFromSystem(FLBuffer buffer)
        {
            if (ActiveBuffer == buffer)
            {
                ActiveBuffer = null;
            }

            if (Input == buffer)
            {
                Input = null;
            }

            internalBuffers.Remove(buffer);

            string[] keys = DefinedBuffers.Where(x => x.Value == buffer).Select(x => x.Key).ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                DefinedBuffers.Remove(keys[i]);
                InternalState.Remove(keys[i]);
            }
        }

        public void FreeResources()
        {
            foreach (KeyValuePair<string, FLBuffer> definedBuffer in DefinedBuffers)
            {
                definedBuffer.Value.Dispose();
            }

            foreach (FLBuffer internalBuffer in internalBuffers)
            {
                internalBuffer.Dispose();
            }

            activeChannelBuffer?.Dispose();
            internalBuffers.Clear();
        }

        public void PushContext()
        {
            ContextStack.Push(new FLExecutionContext(new List<byte>(ActiveChannels).ToArray(), ActiveBuffer, Input));
            ActiveChannels = new byte[] { 1, 1, 1, 1 };
        }

        public void ReturnFromContext()
        {
            FLExecutionContext context = ContextStack.Pop();
            Input = DefinedBuffers["in"] = context.InputBuffer;
            ActiveChannels = context.ActiveChannels;
            ActiveBuffer = context.ActiveBuffer;
        }

        public FLBuffer RegisterUnmanagedBuffer(FLBuffer buffer)
        {
            internalBuffers.Add(buffer);
            return buffer;
        }


        public void SetCLVariables(CLAPI instance, FLBuffer input, bool makeInputInternal)
        {
            //Setting Run Dependent Variables.
            Instance = instance;

            Input = ActiveBuffer = DefinedBuffers["in"] = input;

            InternalState["in"] = makeInputInternal;

            warmed = true;
        }

        private bool warmed = false;
        public void SetCLVariablesAndWarm(CLAPI instance, FLBuffer input, bool makeInputInternal, bool warmBuffers)
        {
            SetCLVariables(instance, input, makeInputInternal);
            warmed = true;
            if (!warmBuffers) return;
            Logger.Log(LogType.Log, "Warming Buffers...", 1);
            foreach (KeyValuePair<string, FLBuffer> definedBuffer in DefinedBuffers)
            {
                if (definedBuffer.Value is IWarmable warmable)
                {
                    Debugger?.ProcessEvent(definedBuffer.Value);
                    warmable.Warm();
                }
            }
            Logger.Log(LogType.Log, "Warming Buffers finished", 1);
        }

        public void Run(CLAPI instance, FLBuffer input, bool makeInputInternal, FLFunction entry = null, bool warmBuffers = false)
        {
            Debugger?.Register(this);
            if (!warmed)
            {
                SetCLVariablesAndWarm(instance, input, makeInputInternal, warmBuffers);
            }
            Input.SetKey("in");
            //Start Setup
            ActiveChannels = new byte[] { 1, 1, 1, 1 };

            FLFunction entryPoint = entry ?? EntryPoint;
            entryPoint.Process();

            warmed = false;

            if (entryPoint.Name == "Main")
                Debugger?.ProgramExit(this);
        }

    }
}