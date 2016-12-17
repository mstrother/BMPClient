using System;
using BMPClient.BGP;

namespace BMPClient
{
    public class BGPNotification : ImsgBody
    {
        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}