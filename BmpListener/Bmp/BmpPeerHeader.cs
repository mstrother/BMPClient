using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bmp
{
    public class PeerHeader
    {
        public PeerHeader(byte[] data)
        {
            Decode(data);
        }

        public PeerType PeerType { get; private set; }
        public bool IsPostPolicy { get; private set; }
        public ulong PeerDistinguisher { get; private set; }
        public IPAddress PeerAddress { get; private set; }
        public uint PeerAS { get; private set; }
        public IPAddress PeerBGPId { get; private set; }
        public DateTime Timestamp { get; private set; }

        public void Decode(byte[] data)
        {
            PeerType = (PeerType) data[0];
            var flags = data[1];
            if ((flags & (1 << 6)) != 0)
                IsPostPolicy = true;

            if ((flags & (1 << 7)) != 0)
            {
                var ipBytes = data.Skip(10).Take(16).ToArray();
                PeerAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = data.Skip(22).Take(4).ToArray();
                PeerAddress = new IPAddress(ipBytes);
            }

            PeerDistinguisher = BitConverter.ToUInt64(data.Skip(2).Take(8).Reverse().ToArray(), 0);
            PeerAS = data.ToUInt32(26);
            PeerBGPId = new IPAddress(data.Skip(30).Take(4).ToArray());

            var seconds = data.ToUInt32(34);
            var microSeconds = data.ToUInt32(38);
            Timestamp =
                DateTimeOffset.FromUnixTimeSeconds(seconds).AddTicks(microSeconds * 10).DateTime.ToUniversalTime();
        }
    }
}