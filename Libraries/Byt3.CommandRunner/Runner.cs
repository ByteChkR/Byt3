using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Byt3.ADL;

namespace Byt3.CommandRunner
{
    /// <summary>
    /// Contains the Logic for Running Commands
    /// </summary>
    public static class Runner
    {
        private static ADLLogger<LogType> Logger => _logger ?? (_logger = new ADLLogger<LogType>(CommandRunnerDebugConfig.Settings, "Runner"));
        private static ADLLogger<LogType> _logger = null;

        /// <summary>
        /// All Commands currently loaded in the Library
        /// </summary>
        private static readonly List<AbstractCommand> Commands = new List<AbstractCommand>();

        /// <summary>
        /// Count of the Loaded Commands.
        /// </summary>
        public static int CommandCount => Commands.Count;

        /// <summary>
        /// Adds an Assemblys Commands by its Full Path
        /// </summary>
        /// <param name="path">Full path to assembly.</param>
        public static void AddAssembly(string path)
        {
            if (AssemblyHelper.TryLoadAssembly(path, out Assembly asm))
            {
                AddAssembly(asm);
            }
        }

        /// <summary>
        /// Adds an Assemblys Commands
        /// </summary>
        /// <param name="asm">Assembly to Add</param>
        public static void AddAssembly(Assembly asm)
        {
            List<AbstractCommand> cmds = AssemblyHelper.LoadCommandsFromAssembly(asm);
            for (int i = 0; i < cmds.Count; i++)
            {
                AddCommand(cmds[i]);
            }
        }

        /// <summary>
        /// Adds a Single Command to the System.
        /// </summary>
        /// <param name="cmd"></param>
        public static void AddCommand(AbstractCommand cmd)
        {
            Logger.Log(LogType.Log, "Adding Command: " + cmd.GetType().FullName, 2);
            if (IsInterfering(cmd))
            {
                Logger.Log(LogType.Log, "Command:" + cmd.GetType().FullName + " is interfering with other Commands.", 1);
            }
            Commands.Add(cmd);
        }

        public static void RemoveAt(int index)
        {
            if (index >= 0 && Commands.Count > index)
            {
                Commands.RemoveAt(index);
            }
        }

        public static void RemoveAllCommands()
        {
            Commands.Clear();
        }

        /// <summary>
        /// Checks if the system is already containing a command with the same command keys
        /// </summary>
        /// <param name="cmd">The Command</param>
        /// <returns>Returns true when interfering with other commands.</returns>
        private static bool IsInterfering(AbstractCommand cmd)
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                if (cmd.IsInterfering(Commands[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the command at index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>Command at index.</returns>
        public static AbstractCommand GetCommandAt(int index)
        {
            return Commands[index];
        }

        /// <summary>
        /// Runs the Commands with the Passed arguments.
        /// </summary>
        /// <param name="args">The arguments to use</param>
        public static bool RunCommands(string[] args)
        {
            bool didExecute = false;
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < Commands.Count; j++)
                {
                    if (Commands[j].CommandKeys.Contains(args[i]))
                    {
                        args[i] = Commands[j].CommandKeys[0]; //Make sure its the first command key.
                    }
                }
            }

            StartupArgumentInfo argumentInfo = new StartupArgumentInfo(args);


            if (argumentInfo.GetCommandEntries("noflag") != 0 ||
                argumentInfo.GetCommandEntries("noflag") == 0 && argumentInfo.CommandCount == 0)
            {
                List<AbstractCommand> cmds = Commands.Where(x => x.DefaultCommand).ToList();
                if (cmds.Count == 0)
                {
                    Logger.Log(LogType.Warning, "No Default Command Found", 1);
                    return didExecute;
                }
                didExecute = true;

                if (cmds.Count != 1)
                {
                    Logger.Log(LogType.Warning, "Found more than one Default Command.", 1);
                    Logger.Log(LogType.Warning, "Using Command: " + cmds[0].CommandKeys[0], 1);
                }

                if (argumentInfo.GetCommandEntries("noflag") != 0)
                {
                    for (int j = 0; j < argumentInfo.GetCommandEntries("noflag"); j++)
                    {
                        cmds[0].CommandAction?.Invoke(argumentInfo, argumentInfo.GetValues("noflag", j).ToArray());
                    }
                }
                else
                {
                    cmds[0].CommandAction?.Invoke(argumentInfo, new string[0]);
                }
            }

            for (int i = 0; i < Commands.Count; i++)
            {
                if (argumentInfo.GetCommandEntries(Commands[i].CommandKeys[0]) != 0)
                {
                    for (int j = 0; j < argumentInfo.GetCommandEntries(Commands[i].CommandKeys[0]); j++)
                    {
                        didExecute = true;
                        Commands[i].CommandAction?.Invoke(argumentInfo,
                            argumentInfo.GetValues(Commands[i].CommandKeys[0], j).ToArray());
                    }
                }
            }

            return didExecute;
        }
    }
}