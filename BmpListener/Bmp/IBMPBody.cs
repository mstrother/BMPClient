using System;

namespace BmpListener.Bmp
{
    public interface IBMPBody
    {
        void ParseBody(BmpMessage message, ArraySegment<byte> messageBytes);
    }
}