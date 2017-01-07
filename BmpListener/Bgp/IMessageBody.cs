using System;

namespace BmpListener.Bgp
{
    public interface IMessageBody
    {
        void DecodeFromBytes(ArraySegment<byte> data);
    }
}