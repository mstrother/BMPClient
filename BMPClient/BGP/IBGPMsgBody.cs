using System;

namespace BMPClient.BGP
{
    public interface IBGPMsgBody
    {
        void DecodeFromBytes(ArraySegment<byte> data);
    }
}