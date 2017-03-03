using System;
using System.Net;

namespace BmpListener.Bgp
{
    public class PathAttributeAggregator : PathAttribute
    {
        public PathAttributeAggregator(ArraySegment<byte> data) : base(data)
        {
            Decode(AttributeValue);
        }

        public int AS { get; private set; }
        public IPAddress IPAddress { get; private set; }

        protected void Decode(ArraySegment<byte> data)
        {
            // TODO: fix
            //AS = BitConverter.ToInt16(data, 0);
            //if (data.Length == 6)
            //{
            //    var ipBytes = data.Skip(2).ToArray();
            //    IPAddress = new IPAddress(ipBytes);
            //}
            //else
            //{
            //    var ipBytes = data.Skip(4).ToArray();
            //    IPAddress = new IPAddress(ipBytes);
            //}
        }
    }
}