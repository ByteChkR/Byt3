using System;
using System.IO;
using Byt3.ADL.Streams;

namespace Byt3.ADL.Network.Client.Streams
{
    public class NetLogStream : LogStream
    {

        /// <summary>
        ///     Creates a NetworkLogStream that uses TCP Clients as input
        /// </summary>
        /// <param name="s"></param>
        /// <param name="mask"></param>
        /// <param name="mt"></param>
        /// <param name="hasTimestamp"></param>
        public NetLogStream(Stream s, int mask, MatchType mt, bool hasTimestamp) :
            base(s, mask, mt, hasTimestamp)
        {



        }




        public override void Write(Log log)
        {
            if (IsClosed) return;
            if (AddTimeStamp) log.Message = Utils.TimeStamp + log.Message;
            var buffer = log.Serialize();
            try
            {
                BaseStream.Write(buffer, 0, buffer.Length);
                Flush();
            }
            catch (Exception)
            {
                Close();
            }
}
    }
}