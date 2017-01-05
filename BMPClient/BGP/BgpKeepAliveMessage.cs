using System;

namespace BmpListener.Bgp
{
    public class BgpKeepAliveMessage : IMessageBody
    {
        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}
