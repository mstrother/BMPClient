using System;

namespace BmpListener.Bmp
{
    public class BmpInitiation : BmpMessage
    {
        public BmpInitiation(BmpHeader bmpHeader)
            : base(bmpHeader)
        {
            BmpVersion = BmpHeader.Version;
        }
        
        public int BmpVersion { get; }
    }
}