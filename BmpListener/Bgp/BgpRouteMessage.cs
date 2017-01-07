using System;

namespace BmpListener.Bgp
{
    public class BgpRouteMessage : IMessageBody
    {
        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}