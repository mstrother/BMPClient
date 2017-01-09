using System;

namespace BmpListener.Bmp
{
    public class StatisticsReport : IBMPBody
    {
        public void ParseBody(BmpMessage message, ArraySegment<byte> messageBytes)
        {
            throw new NotImplementedException();
        }

        public BmpMessage ParseBody()
        {
            throw new NotImplementedException();
        }
    }
}