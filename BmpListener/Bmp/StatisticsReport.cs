using System;

namespace BmpListener.Bmp
{
    public class StatisticsReport : BmpMessage
    {
        public StatisticsReport(BmpHeader bmpHeader, byte[] data)
            : base(bmpHeader, data)
        {
        }        
    }
}