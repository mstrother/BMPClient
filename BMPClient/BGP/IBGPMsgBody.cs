using System;

namespace BmpListener.BGP
{
    public interface IBGPMsgBody
    {
        void DecodeFromBytes(ArraySegment<byte> data);
    }
}