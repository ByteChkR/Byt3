using System;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    [Flags]
    public enum FLProgramCheckType
    {
        None = 0,
        InputValidation = 1,
        Optimization = 2,
        AggressiveOptimization = 4,


        InputValidationOptimized = InputValidation | Optimization,
        InputValidationAggressive = InputValidation | AggressiveOptimization,
        AllOptimizations = Optimization | AggressiveOptimization,
        All = InputValidation | Optimization | AggressiveOptimization

        //Helpers for the Debugger to display the combinations easily
    }
}