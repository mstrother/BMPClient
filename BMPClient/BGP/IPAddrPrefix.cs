using System;
using System.Linq;
using System.Net;

namespace BmpListener.BGP
{
    public class IPAddrPrefix
    {
        public IPAddrPrefix(ArraySegment<byte> data, BGP.AddressFamily afi = BGP.AddressFamily.IPv4)
        {
            DecodeFromBytes(data, afi);
        }

        public byte Length { get; private set; }
        public IPAddress Prefix { get; private set; }

        public void DecodeFromBytes(ArraySegment<byte> data, BGP.AddressFamily afi)
        {
            //add length error check
            Length = data.ElementAt(0);
            var byteLength = (Length + 7) / 8;
            var ipBytes = afi == BGP.AddressFamily.IPv4
                ? new byte[4]
                : new byte[16];
            if (Length <= 0) return;
            Buffer.BlockCopy(data.ToArray(), 1, ipBytes, 0, byteLength);
            Prefix = new IPAddress(ipBytes);
        }
    }
}
