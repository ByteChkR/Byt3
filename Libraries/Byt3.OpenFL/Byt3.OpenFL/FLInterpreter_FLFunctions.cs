using System;
using Byt3.OpenCL.Common;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.Wrapper;

namespace Byt3.OpenFL
{
    /// <summary>
    /// Partial Class that Contains the Baked FL Functions
    /// </summary>
    public partial class FLInterpreter
    {
        /// <summary>
        /// The implementation of the command setactive
        /// </summary>
        private void cmd_setactive()
        {
            if (currentArgStack.Count < 1)
            {
                CLLogger.Crash(new FLInvalidFunctionUseException("setactive", "Specify the buffer you want to activate"),
                    true);
                return;
            }

            byte[] temp = new byte[channelCount];
            while (currentArgStack.Count != 1)
            {
                object val = currentArgStack.Pop();
                if (!(val is decimal))
                {
                    CLLogger.Crash(new FLInvalidFunctionUseException("setactive", "Invalid channel Arguments"), true);
                    val = 0;
                }

                byte channel = (byte) Convert.ChangeType(val, typeof(byte));
                if (channel >= channelCount)
                {
                    CLLogger.Log("Script is enabling channels beyond channel count. Ignoring...",
                        DebugChannel.Warning | DebugChannel.EngineOpenFL, 10);
                }
                else
                {
                    temp[channel] = 1;
                }
            }

            if (currentArgStack.Peek() == null ||
                !(currentArgStack.Peek() is CLBufferInfo) && !(currentArgStack.Peek() is decimal))
            {
                CLLogger.Crash(new FLInvalidFunctionUseException("setactive", "Specify the buffer you want to activate"),
                    true);
                return;
            }

            if (currentArgStack.Peek() is decimal)
            {
                byte channel = (byte) Convert.ChangeType(currentArgStack.Pop(), typeof(byte));
                temp[channel] = 1;
            }
            else
            {
                currentBuffer = (CLBufferInfo) currentArgStack.Pop();
            }

            bool needCopy = false;
            for (int i = 0; i < channelCount; i++)
            {
                if (activeChannels[i] != temp[i])
                {
                    needCopy = true;
                    break;
                }
            }

            if (needCopy)
            {
                CLLogger.Log("Updating Channel Buffer", DebugChannel.Log | DebugChannel.EngineOpenFL, 6);
                activeChannels = temp;
                CLAPI.WriteToBuffer(instance, activeChannelBuffer, activeChannels);
            }
            else
            {
                CLLogger.Log("Skipping Updating Channel Buffer", DebugChannel.Log | DebugChannel.EngineOpenFL, 6);
            }
        }

        /// <summary>
        /// A function used as RandomFunc of type byte>
        /// </summary>
        /// <returns>a random byte</returns>
        private static byte Randombytesource()
        {
            return (byte) Rnd.Next();
        }

        /// <summary>
        /// The implementation of the command random
        /// </summary>
        private void cmd_writerandom()
        {
            if (currentArgStack.Count == 0)
            {
                CLAPI.WriteRandom(instance, currentBuffer.Buffer, Randombytesource, activeChannels, false);
            }

            while (currentArgStack.Count != 0)
            {
                object obj = currentArgStack.Pop();
                if (!(obj is CLBufferInfo))
                {
                    CLLogger.Crash(
                        new FLInvalidArgumentType("Argument: " + currentArgStack.Count + 1, "MemoyBuffer/Image"),
                        true);
                    continue;
                }

                CLAPI.WriteRandom(instance, (obj as CLBufferInfo).Buffer, Randombytesource, activeChannels, false);
            }
        }

        /// <summary>
        /// The implementation of the command random
        /// </summary>
        private void cmd_writerandomu()
        {
            if (currentArgStack.Count == 0)
            {
                CLAPI.WriteRandom(instance, currentBuffer.Buffer, Randombytesource, activeChannels, true);
            }

            while (currentArgStack.Count != 0)
            {
                object obj = currentArgStack.Pop();
                if (!(obj is CLBufferInfo))
                {
                    CLLogger.Crash(
                        new FLInvalidArgumentType("Argument: " + currentArgStack.Count + 1, "MemoyBuffer/Image"),
                        true);
                    continue;
                }

                CLAPI.WriteRandom(instance, (obj as CLBufferInfo).Buffer, Randombytesource, activeChannels, true);
            }
        }

        /// <summary>
        /// The implementation of the command jmp
        /// </summary>
        private void cmd_jump() //Dummy function. Implementation in AnalyzeLine(code) function(look for isDirectExecute)
        {
            CLLogger.Log("Jumping.", DebugChannel.Log | DebugChannel.EngineOpenFL, 6);
        }

        /// <summary>
        /// The implementation of the command brk
        /// </summary>
        private void cmd_break()
        {
            if (ignoreDebug)
            {
                return;
            }

            stepResult.TriggeredDebug = true;
            if (currentArgStack.Count == 0)
            {
                stepResult.DebugBufferName = currentBuffer.ToString();
            }
            else if (currentArgStack.Count == 1)
            {
                object obj = currentArgStack.Pop();
                if (!(obj is CLBufferInfo))
                {
                    CLLogger.Crash(
                        new FLInvalidArgumentType("Argument: " + currentArgStack.Count + 1, "MemoyBuffer/Image"),
                        true);
                    return;
                }

                stepResult.DebugBufferName = (obj as CLBufferInfo).ToString();
            }
            else
            {
                CLLogger.Crash(new FLInvalidFunctionUseException("Break", "only one or zero arguments"), true);
            }
        }
    }
}