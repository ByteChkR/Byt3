namespace Byt3.OpenFL
{
    /// <summary>
    /// Enum that is used as a return for the Line Analysis of the FL Interpreter
    /// </summary>
    public enum FLLineAnalysisResult
    {
        /// <summary>
        /// Everything worked, Increase the Program Counter
        /// </summary>
        IncreasePc,

        /// <summary>
        /// We encountered a problem(log the problem and continue if recoverable
        /// </summary>
        ParseError,

        /// <summary>
        /// We are jumping from the current source line to somewhere else
        /// </summary>
        Jump
    }
}