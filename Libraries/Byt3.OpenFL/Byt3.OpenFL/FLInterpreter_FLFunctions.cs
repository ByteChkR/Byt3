using System;
using Byt3.ADL;
using Byt3.OpenCL.Common.Exceptions;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Exceptions;

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
        private void CmdSetActive()
        {
            if (currentArgStack.Count < 1)
            {
                throw new FLInvalidFunctionUseException("setactive", "Specify the buffer you want to activate");

            }

            byte[] temp = new byte[channelCount];
            while (currentArgStack.Count != 1)
            {
                object val = currentArgStack.Pop();
                if (!(val is decimal))
                {
                    throw new FLInvalidFunctionUseException("setactive", "Invalid channel Arguments");
                }

                byte channel = (byte) Convert.ChangeType(val, typeof(byte));
                if (channel >= channelCount)
                {
                    Logger.Log(LogType.Error, 
                        "Script is enabling channels beyond channel count. Ignoring...",1);
                }
                else
                {
                    temp[channel] = 1;
                }
            }

            if (currentArgStack.Peek() == null ||
                !(currentArgStack.Peek() is CLBufferInfo) && !(currentArgStack.Peek() is decimal))
            {
                throw new FLInvalidFunctionUseException("setactive", "Specify the buffer you want to activate");
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
                activeChannels = temp;
                UpdateActiveChannels();
            }
            else
            {
                Logger.Log(LogType.Log,  "Skipping Updating Channel Buffer", 1);
            }
        }

        private void UpdateActiveChannels()
        {
            Logger.Log(LogType.Log,  "Updating Channel Buffer", 1);
            CLAPI.WriteToBuffer(instance, activeChannelBuffer, activeChannels);
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
        private void CmdWriteRandom()
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
                    throw
                        new FLInvalidArgumentType("Argument: " + currentArgStack.Count + 1, "MemoyBuffer/Image");
                }

                CLAPI.WriteRandom(instance, (obj as CLBufferInfo).Buffer, Randombytesource, activeChannels, false);
            }
        }

        /// <summary>
        /// The implementation of the command random
        /// </summary>
        private void CmdWriteRandomU()
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
                    throw new FLInvalidArgumentType("Argument: " + currentArgStack.Count + 1, "MemoyBuffer/Image");
                }

                CLAPI.WriteRandom(instance, (obj as CLBufferInfo).Buffer, Randombytesource, activeChannels, true);
            }
        }

        /// <summary>
        /// The implementation of the command jmp
        /// </summary>
        private void CmdJump() //Dummy function. Implementation in AnalyzeLine(code) function(look for isDirectExecute)
        {
            Logger.Log(LogType.Log,  "Jumping.", 1);
        }

        /// <summary>
        /// The implementation of the command brk
        /// </summary>
        private void CmdBreak()
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
                    throw new FLInvalidArgumentType("Argument: " + currentArgStack.Count + 1, "MemoyBuffer/Image");
                }

                stepResult.DebugBufferName = (obj as CLBufferInfo).ToString();
            }
            else
            {
                throw new FLInvalidFunctionUseException("Break", "only one or zero arguments");
            }
        }
    }
}