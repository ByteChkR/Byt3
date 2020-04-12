namespace Byt3.ADL
{
    /// <summary>
    /// An enum used to give logs an importance.
    /// </summary>
    public enum Verbosity
    {
        /// <summary>
        /// Lowest Verbosity Level, no output on console.
        /// </summary>
        Silent = 0,
        /// <summary>
        /// Only critical errors and general information
        /// </summary>
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        /// <summary>
        /// Highest Level of verbosity, you will get every log that gets sent.
        /// </summary>
        Level8
    }
}