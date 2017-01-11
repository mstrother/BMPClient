using System;

namespace BmpListener.Bmp
{
    public interface IBMPBody
    {
        void ParseBody(ArraySegment<byte> messageBytes);
    }
}