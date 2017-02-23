using System;
using System.Net;

namespace BmpListener.Bgp
{
    public class IPAddrPrefix
    {
        public IPAddrPrefix(byte[] data, AddressFamily afi = AddressFamily.IP)
        {
            DecodeFromBytes(data, afi);
        }

        public byte Length { get; private set; }
        public IPAddress Prefix { get; private set; }

        public override string ToString()
        {
            return ($"{Prefix}/{Length}");
        }

        public int GetByteLength()
        {
            return 1 + (Length + 7) / 8;
        }
        
        public void DecodeFromBytes(byte[] data, AddressFamily afi)
        {
            Length = data[0];
            if (Length <= 0) return;
            var byteLength = (Length + 7) / 8;
            var ipBytes = afi == AddressFamily.IP
                ? new byte[4]
                : new byte[16];
            Buffer.BlockCopy(data, 1, ipBytes, 0, byteLength);
            Prefix = new IPAddress(ipBytes);
        }
    }
}
