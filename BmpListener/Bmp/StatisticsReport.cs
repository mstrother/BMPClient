using System;

namespace BmpListener.Bmp
{
    public class StatisticsReport : BmpMessage
    {
        public StatisticsReport(BmpHeader bmpHeader, byte[] data)
            : base(bmpHeader, data)
        {
            throw new NotImplementedException();
        }

        public BmpMessage ParseBody()
        {
            throw new NotImplementedException();
        }
    }
}