using System;
using System.Net;

namespace BmpListener.Bmp
{
    public class PerPeerHeader
    {
        private const long TicksPerMicrosecond = 10;
        
        public PerPeerHeader(byte[] data)
        {
            Decode(data);
        }

        public enum Type
        {
            Global,
            RD,
            Local
        }

        public byte Flags { get; private set; }
        public Type PeerType { get; private set; }
        public bool IsPostPolicy { get; private set; }
        public ulong PeerDistinguisher { get; private set; }
        public IPAddress PeerAddress { get; private set; }
        public int AS { get; private set; }
        public IPAddress PeerBGPId { get; private set; }
        public DateTimeOffset DateTime { get; private set; }

        public void Decode(byte[] data)
        {
            PeerType = (Type)data[0];

            Flags = data[1];
            if ((Flags & (1 << 6)) != 0)
            {
                IsPostPolicy = true;
            }

            Array.Reverse(data, 2, 8);
            PeerDistinguisher = BitConverter.ToUInt64(data, 2);

            if ((Flags & (1 << 7)) != 0)
            {
                var ipBytes = new byte[16];
                Array.Copy(data, 10, ipBytes, 0, 16);
                PeerAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data, 22, ipBytes, 0, 4);
                PeerAddress = new IPAddress(ipBytes);
            }

            Array.Reverse(data, 26, 4);
            AS = BitConverter.ToInt32(data, 26);

            var peerIdBytes = new byte[4];
            Array.Copy(data, 30, peerIdBytes, 0, 4);
            PeerBGPId = new IPAddress(peerIdBytes);

            Array.Reverse(data, 34, 4);
            var seconds = BitConverter.ToInt32(data, 34);

            Array.Reverse(data, 38, 4);
            var microSeconds = BitConverter.ToInt32(data, 38);

            var ticks = microSeconds * TicksPerMicrosecond;
            DateTime = DateTimeOffset.FromUnixTimeSeconds(seconds)
                .AddTicks(ticks).UtcDateTime;
        }
    }
}