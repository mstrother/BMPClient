using System;
using System.Linq;
using System.Net;

namespace BMPClient.BMP
{
    public class PeerHeader
    {
        public PeerHeader(byte[] data)
        {
            Decode(data);
        }

        public byte PeerType { get; private set; }
        public bool IsPostPolicy { get; private set; }
        public ulong PeerDistinguisher { get; private set; }
        public IPAddress PeerAddress { get; private set; }
        public uint PeerAS { get; private set; }
        public IPAddress PeerBGPId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public byte Flags { get; private set; }

        public void Decode(byte[] data)
        {
            PeerType = data[0];
            Flags = data[1];
            if ((Flags & (1 << 6)) != 0)
                IsPostPolicy = true;

            if ((Flags & (1 << 7)) != 0)
            {
                var ipBytes = new byte[16];
                Buffer.BlockCopy(data, 10, ipBytes, 0, 16);
                PeerAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Buffer.BlockCopy(data, 22, ipBytes, 0, 4);
                PeerAddress = new IPAddress(ipBytes);
            }
            
            PeerDistinguisher = BitConverter.ToUInt64(data.Skip(2).Take(8).Reverse().ToArray(), 0);
            PeerAS = data.ToUInt32(26);

            var bytes = new byte[4];
            Buffer.BlockCopy(data, 30, bytes, 0, 4);
            PeerBGPId = new IPAddress(bytes);

            var seconds = data.ToUInt32(34);
            var microSeconds = data.ToUInt32(38);
            Timestamp =
                DateTimeOffset.FromUnixTimeSeconds(seconds).AddTicks(microSeconds * 10).DateTime.ToUniversalTime();
        }
    }
}