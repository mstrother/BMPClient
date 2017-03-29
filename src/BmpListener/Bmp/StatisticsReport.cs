using System;

namespace BmpListener.Bmp
{
    public class StatisticsReport : BmpMessage
    {
        public StatisticsReport()
        {
        }

        public override void Decode(byte[] data, int offset)
        {
            throw new NotImplementedException();
        }
    }
}