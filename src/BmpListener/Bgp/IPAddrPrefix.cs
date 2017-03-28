using System;
using System.Net;

namespace BmpListener.Bgp
{
    public class IPAddrPrefix
    {
        public IPAddrPrefix(byte[] data, int offset, AddressFamily afi = AddressFamily.IP)
        {
            DecodeFromBytes(data, offset, afi);
        }

        internal int ByteLength { get { return 1 + (Length + 7) / 8; } }
        public int Length { get; private set; }
        public IPAddress Prefix { get; private set; }

        public override string ToString()
        {
            return ($"{Prefix}/{Length}");
        }

        public void DecodeFromBytes(byte[] data, int offset, AddressFamily afi = AddressFamily.IP)
        {
            Length = data[offset];
            var byteLength = (Length + 7) / 8;
            var ipBytes = afi == AddressFamily.IP
                ? new byte[4]
                : new byte[16];
            Array.Copy(data, offset + 1, ipBytes, 0, byteLength);
            Prefix = new IPAddress(ipBytes);
        }
    }
}
