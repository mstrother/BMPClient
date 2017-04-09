using BmpListener.MiscUtil.Conversion;
using System;
using System.Linq;
using System.Net;
//using BmpListener.MiscUtil.Conversion;

namespace BmpListener.Bmp
{
    public class PerPeerHeader
    {
        private const long TicksPerMicrosecond = 10;

        public PerPeerHeader()
        { }
        
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
        public IPAddress PeerId { get; private set; }
        public DateTimeOffset DateTime { get; private set; }

        public void Decode(byte[] data, int offset)
        {
            PeerType = (Type)data[offset];
            Flags = data[offset + 1];
            if ((Flags & (1 << 6)) != 0)
            {
                IsPostPolicy = true;
            }

            PeerDistinguisher = EndianBitConverter.Big.ToUInt64(data, 2);
            
            if ((Flags & (1 << 7)) != 0)
            {
                var ipBytes = new byte[16];
                Array.Copy(data, offset + 10, ipBytes, 0, 4);
                PeerAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data, offset + 22, ipBytes, 0, 4);
                PeerAddress = new IPAddress(ipBytes);
            }

            AS = EndianBitConverter.Big.ToInt32(data, 26);

            var peerIdBytes = new byte[4];
            Array.Copy(data, offset + 30, peerIdBytes, 0, 4);
            PeerId = new IPAddress(peerIdBytes);

            var seconds = EndianBitConverter.Big.ToInt32(data, 34);
            var microSeconds = EndianBitConverter.Big.ToInt32(data, 38);

            var ticks = microSeconds * TicksPerMicrosecond;
            DateTime = DateTimeOffset.FromUnixTimeSeconds(seconds)
                .AddTicks(ticks).UtcDateTime;
        }
    }
}