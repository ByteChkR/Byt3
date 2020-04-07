namespace Byt3.OpenCL.Common
{
    ///// <summary>
    ///// Helper Class that Communicates with the ADL Library
    ///// </summary>
    //public static class DebugHelper
    //{
    //    public static int SeverityFilter;
    //    public static bool ThrowOnAllExceptions = true;
    //    private static readonly ADLLogger<DebugChannel> _logger = new ADLLogger<DebugChannel>("OpenCL/FL");


    //    public static void Crash(Exception ex, bool recoverable)
    //    {
    //        CrashHandler.Log(ex, true);

    //        if (!recoverable || ThrowOnAllExceptions)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static void Log(string message, int channel)
    //    {
    //        Log(message, channel, 0);
    //    }

    //    public static void Log(string message, int channel, int severity)
    //    {
    //        if (severity < SeverityFilter)
    //        {
    //            return;
    //        }

    //        _logger.Log(channel, $"[S:{severity}]\t" + message);
    //    }
    //}
}