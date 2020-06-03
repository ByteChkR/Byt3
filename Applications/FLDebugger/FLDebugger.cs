﻿using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using FLDebugger.Forms;

namespace FLDebugger
{
    public class FLDebugger : IDebugger
    {
        public delegate IProgramDebugger CreateDebugger(FLProgram program);

        private readonly CreateDebugger debuggerCreator;

        private readonly Dictionary<FLProgram, IProgramDebugger> Debuggers =
            new Dictionary<FLProgram, IProgramDebugger>();

        public FLDebugger(CreateDebugger creator)
        {
            debuggerCreator = creator;
        }


        public void Register(FLProgram program)
        {
            Debuggers.Add(program, debuggerCreator(program));
        }

        public void ProcessEvent(FLParsedObject obj)
        {
            FLProgram rt = obj.Root;
            if (Debuggers.ContainsKey(rt))
            {
                Debuggers[rt].ProcessEvent(obj);
            }
        }

        public void ProgramStart(FLProgram program)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].ProgramStart();
            }
        }

        public void ProgramExit(FLProgram program)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].ProgramExit();
                Debuggers.Remove(program);
            }
        }

        public void SubProgramStarted(FLProgram program, ExternalFlFunction subProgram, FLProgram script)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].ProcessEvent(subProgram);
                Debuggers[program].SubProgramStarted();
                if (Debuggers[program].FollowScripts)
                {
                    Register(script);
                    ProgramStart(script);
                }
            }
        }

        public void SubProgramEnded(FLProgram program, FLProgram script)
        {
            if (Debuggers.ContainsKey(program))
            {
                if (Debuggers[program].FollowScripts)
                {
                    ProgramExit(script);
                }

                Debuggers[program].SubProgramEnded();
            }
        }

        public void OnAddInternalBuffer(FLProgram program, FLBuffer internalBuffer)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnAddInternalBuffer(internalBuffer);
            }
        }


        public static void Initialize()
        {
            if (FLProgram.Debugger != null && FLProgram.Debugger.GetType() == typeof(FLDebugger))
            {
                return;
            }

            FLProgram.Debugger = new FLDebugger(program =>
            {
                FLDebuggerWindow cv = new FLDebuggerWindow(program);
                return cv;
            });
        }

        public static void Start(CLAPI instance, FLProgram program, int width, int height, int depth)
        {
            FLProgram.Debugger?.Register(program);
            program.Run(new FLBuffer(instance, width, height, depth, "DebugInput"), true);
            program.FreeResources();
        }
    }
}