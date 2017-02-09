using BmpListener.Extensions;
using System;
using System.Linq;
using System.Net;

namespace BmpListener.Bmp
{
    public class BmpPeerHeader
    {
        public BmpPeerHeader(ArraySegment<byte> data)
        {
            Decode(data);
        }

        public enum Type
        {
            Global,
            RD,
            Local
        }

        public Type PeerType { get; private set; }
        public bool IsPostPolicy { get; private set; }
        public ulong PeerDistinguisher { get; private set; }
        public IPAddress PeerAddress { get; private set; }
        public int AS { get; private set; }
        public IPAddress PeerBGPId { get; private set; }
        public DateTime DateTime { get; private set; }

        public void Decode(ArraySegment<byte> data)
        {
            PeerType = (Type)data.First();
            var flags = data.ElementAt(1);
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
            AS = data.ToInt32(26);
            PeerBGPId = new IPAddress(data.Skip(30).Take(4).ToArray());

            var seconds = data.ToInt32(34);
            var microSeconds = data.ToInt32(38);
            DateTime =
                DateTimeOffset.FromUnixTimeSeconds(seconds).AddTicks(microSeconds * 10).DateTime.ToUniversalTime();
        }
    }
}