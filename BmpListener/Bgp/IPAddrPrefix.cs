using System;
using System.Net;

namespace BmpListener.Bgp
{
    public class IPAddrPrefix
    {
        public IPAddrPrefix(ArraySegment<byte> data, AddressFamily afi = AddressFamily.IP)
        {
            Length = data.Array[data.Offset];
            var ipBytes = new byte[ByteLength - 1];
            Array.Copy(data.Array, data.Offset + 1, ipBytes, 0, ipBytes.Length);
            DecodeFromBytes(ipBytes, afi);
        }

        internal int ByteLength { get { return 1 + (Length + 7) / 8; } }
        public int Length { get; private set; }
        public IPAddress Prefix { get; private set; }

        public override string ToString()
        {
            return ($"{Prefix}/{Length}");
        }

        public void DecodeFromBytes(byte[] data, AddressFamily afi = AddressFamily.IP)
        {
            var ipBytes = afi == AddressFamily.IP
                ? new byte[4]
                : new byte[16];
            Array.Copy(data, 0, ipBytes, 0, data.Length);
            Prefix = new IPAddress(ipBytes);
        }
    }
}
