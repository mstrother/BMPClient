using System;
using System.Linq;
using System.Net;

namespace BmpListener.Bmp
{
    public class PerPeerHeader
    {
        private const long TicksPerMicrosecond = 10;

        public PerPeerHeader(byte[] data, int offset)
        {
            Decode(data, offset);
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

        public void Decode(byte[] data, int offset)
        {
            var b64 = Convert.ToBase64String(data.Skip(offset).ToArray());
            PeerType = (Type)data[offset];
            offset++;

            Flags = data[offset];
            if ((Flags & (1 << 6)) != 0)
            {
                IsPostPolicy = true;
            }
            offset++;

            Array.Reverse(data, offset, offset + 8);
            PeerDistinguisher = BitConverter.ToUInt64(data, offset);
            offset += 8;

            if ((Flags & (1 << 7)) != 0)
            {
                var ipBytes = new byte[16];
                Array.Copy(data, offset, ipBytes, 0, 16);
                PeerAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data, offset + 12, ipBytes, 0, 4);
                PeerAddress = new IPAddress(ipBytes);
            }
            offset += 16;

            Array.Reverse(data, offset, 4);
            AS = BitConverter.ToInt32(data, offset);
            offset += 4;

            var peerIdBytes = new byte[4];
            Array.Copy(data, offset, peerIdBytes, 0, 4);
            PeerBGPId = new IPAddress(peerIdBytes);
            offset += 4;

            Array.Reverse(data, offset, 4);
            var seconds = BitConverter.ToInt32(data, 34);
            offset += 4;

            Array.Reverse(data, offset, 4);
            var microSeconds = BitConverter.ToInt32(data, 38);

            var ticks = microSeconds * TicksPerMicrosecond;
            DateTime = DateTimeOffset.FromUnixTimeSeconds(seconds)
                .AddTicks(ticks).UtcDateTime;
        }
    }
}