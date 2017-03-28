using System;

namespace BmpListener.Bmp
{
    public class StatisticsReport : BmpMessage
    {
        public StatisticsReport(byte[] data)
            : base(data)
        {
        }        
    }
}