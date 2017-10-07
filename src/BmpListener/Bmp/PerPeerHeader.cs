using System;
using System.Net;
using BmpListener.Utilities;

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
            Local,
            LocalRIB
        }

        public byte Flags { get; private set; }
        public Type PeerType { get; private set; }
        public bool IsPostPolicy { get; private set; }
        public ulong PeerDistinguisher { get; private set; }
        public IPAddress PeerAddress { get; private set; }
        public int AS { get; private set; }
        public IPAddress PeerId { get; private set; }
        public DateTime DateTime { get; private set; }

        public void Decode(byte[] data, int offset)
        {
            PeerType = (Type)data[offset];
            offset++;

            Flags = data[offset];
            offset++;
            if ((Flags & (1 << 6)) != 0)
            {
                IsPostPolicy = true;
            }

            PeerDistinguisher = EndianBitConverter.Big.ToUInt64(data, offset);
            offset += 8;
            
            if ((Flags & (1 << 7)) != 0)
            {
                var ipBytes = new byte[16];
                Array.Copy(data, offset, ipBytes, 0, 4);
                PeerAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data, offset + 12, ipBytes, 0, 4);
                PeerAddress = new IPAddress(ipBytes);
            }
            offset += 16;

            AS = EndianBitConverter.Big.ToInt32(data, offset);
            offset += 4;

            var peerIdBytes = new byte[4];
            Array.Copy(data, offset, peerIdBytes, 0, 4);
            PeerId = new IPAddress(peerIdBytes);
            offset += 4;

            var seconds = EndianBitConverter.Big.ToInt32(data, offset);
            offset += 4;
            var microSeconds = EndianBitConverter.Big.ToInt32(data, offset);

            var ticks = microSeconds * TicksPerMicrosecond;
            DateTime = DateTimeOffset.FromUnixTimeSeconds(seconds)
                .AddTicks(ticks).UtcDateTime;
        }
    }
}