using System;
using System.Net;

namespace BMPClient.BGP
{
    public class IPAddrPrefix
    {
        public IPAddrPrefix(byte[] data)
        {
            DecodeFromBytes(data);
        }

        public byte Length { get; private set; }
        public IPAddress Prefix { get; private set; }

        public void DecodeFromBytes(byte[] data)
        {
            //add length error check
            Length = data[0];
            var byteLen = (Length + 7) / 8;
            var ipBytes = new byte[16];
            Buffer.BlockCopy(data, 1, ipBytes, 0, byteLen);
            Prefix = new IPAddress(ipBytes);
            //GetPrefix()
        }
    }
}