using System;

namespace BmpListener.Bmp
{
    public class BmpTermination : BmpMessage
    {
        public BmpTermination(BmpHeader bmpHeader)
        {
        }

        public override void Decode(byte[] data, int offset)
        {
            throw new NotImplementedException();
        }
    }
}