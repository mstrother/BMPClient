using System;

namespace BmpListener.Bmp
{
    public class BmpTermination : BmpMessage
    {
        public BmpTermination(BmpHeader bmpHeader)
            : base(bmpHeader)
        { }
    }
}