using BmpListener.Extensions;
using BmpListener.Serialization;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeAggregator : PathAttribute
    {
        public PathAttributeAggregator(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromByes(data);
        }

        public int AS { get; private set; }
        public IPAddress IPAddress { get; private set; }

        public void DecodeFromByes(ArraySegment<byte> data)
        {
            if (data.Count == 6)
            {
                AS = (int)data.ToUInt16(0);
                var ipBytes = data.Skip(2).ToArray();
                IPAddress = new IPAddress(ipBytes);
            }
            else
            {
                AS = (int)data.ToUInt32(0);
                var ipBytes = data.Skip(4).ToArray();
                IPAddress = new IPAddress(ipBytes);
            }
        }
    }
}