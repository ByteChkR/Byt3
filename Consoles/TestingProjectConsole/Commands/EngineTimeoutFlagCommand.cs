using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class EngineTimeoutFlagCommand:AbstractCommand
    {

        public EngineTimeoutFlagCommand() : base(new[] {"--engine-timeout", "-eT"},
            "Sets the Engine Timeout in Seconds")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            EngineSceneRunCommand.AttachTimeout = true;
            EngineSceneRunCommand.TimeoutTime = int.Parse(args[0]);
        }
        
    }
}