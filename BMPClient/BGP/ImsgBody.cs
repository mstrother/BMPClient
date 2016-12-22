using System;

namespace BmpListener.BGP
{
    public interface ImsgBody
    {
        void DecodeFromBytes(ArraySegment<byte> data);
    }
}