using System;

namespace BmpListener.Bmp
{
    public class BmpTermination : BmpMessage
    {
        public override void Decode(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}