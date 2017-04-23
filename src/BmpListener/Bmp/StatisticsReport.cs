using System;
using BmpListener.MiscUtil.Conversion;

namespace BmpListener.Bmp
{
    public class StatisticsReport : BmpMessage
    {
        public int Count { get; set; }

        public override void Decode(byte[] data, int offset)
        {
            Count = EndianBitConverter.Big.ToInt32(data, offset);
        }
    }
}