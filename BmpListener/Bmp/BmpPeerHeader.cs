using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bmp
{
    public class BmpPeerHeader
    {
        public BmpPeerHeader(ArraySegment<byte> data)
        {
            Decode(data);
        }

        public PeerType Type { get; private set; }
        public bool IsPostPolicy { get; private set; }
        [JsonProperty(PropertyName = "distinguisher")]
        public ulong PeerDistinguisher { get; private set; }
        [JsonProperty(PropertyName = "ip")]
        public IPAddress PeerAddress { get; private set; }
        [JsonProperty(PropertyName = "as")]
        public uint AS { get; private set; }
        [JsonProperty(PropertyName = "id")]
        public IPAddress PeerBGPId { get; private set; }
        [JsonIgnore]
        public DateTime DateTime { get; private set; }

        public void Decode(ArraySegment<byte> data)
        {
            Type = (PeerType)data.First();
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
            AS = data.ToUInt32(26);
            PeerBGPId = new IPAddress(data.Skip(30).Take(4).ToArray());

            var seconds = data.ToUInt32(34);
            var microSeconds = data.ToUInt32(38);
            DateTime =
                DateTimeOffset.FromUnixTimeSeconds(seconds).AddTicks(microSeconds * 10).DateTime.ToUniversalTime();
        }
    }
}