using System;

namespace BmpListener.Bmp
{
    public class BmpTermination : IBMPBody
    {
        public void ParseBody(BmpMessage message, ArraySegment<byte> messageBytes)
        {
            throw new NotImplementedException();
        }
    }
}