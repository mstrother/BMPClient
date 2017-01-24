using System;

namespace BmpListener.Bmp
{
    public class StatisticsReport : BmpMessage
    {
        public StatisticsReport(BmpHeader bmpHeader, ArraySegment<byte> data)
            : base(bmpHeader, ref data)
        {
            throw new NotImplementedException();
        }

        public BmpMessage ParseBody()
        {
            throw new NotImplementedException();
        }
    }
}