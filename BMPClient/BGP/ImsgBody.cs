using System;

namespace BMPClient.BGP
{
    public interface ImsgBody
    {
        void DecodeFromBytes(ArraySegment<byte> data);
    }
}