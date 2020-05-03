using System.Collections.Generic;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace FLDebugger
{
    public class FLDebugger : IDebugger
    {
        private readonly Dictionary<FLProgram, IProgramDebugger> Debuggers = new Dictionary<FLProgram, IProgramDebugger>();
        private readonly CreateDebugger debuggerCreator;


        public static void Initialize()
        {
            if (FLProgram.Debugger != null && FLProgram.Debugger.GetType() == typeof(FLDebugger)) return;
            FLProgram.Debugger = new FLDebugger(program =>
            {
                CodeView cv = new CodeView(program);
                return cv;
            });
        }

        public static void Start(FLProgram program)
        {
            program.Run(CLAPI.MainThread, new FLBuffer(CLAPI.MainThread, 512, 512, "DebugInput"), true);
            program.FreeResources();
        }

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
                Debuggers[rt].ProcessEvent(obj);
        }

        public void ProgramStart(FLProgram program)
        {
            if (!Debuggers.ContainsKey(program))
            {
                Register(program);
            }
            Debuggers[program].ProgramStart();
        }

        public void ProgramExit(FLProgram program)
        {
            Debuggers[program].ProgramExit();
            Debuggers.Remove(program);
        }

        public void SubProgramStarted(FLProgram program, ExternalFlFunction subProgram, FLProgram script)
        {
            Debuggers[program].ProcessEvent(subProgram);
            Debuggers[program].SubProgramStarted();
            if (Debuggers[program].FollowScripts)
                ProgramStart(script);
        }

        public void SubProgramEnded(FLProgram program, FLProgram script)
        {
            if (Debuggers[program].FollowScripts)
                ProgramExit(script);
            Debuggers[program].SubProgramEnded();
        }

        public void OnAddInternalBuffer(FLProgram program, FLBuffer internalBuffer)
        {
            if (Debuggers.ContainsKey(program))
            {
                Debuggers[program].OnAddInternalBuffer(internalBuffer);
            }
        }
    }
}