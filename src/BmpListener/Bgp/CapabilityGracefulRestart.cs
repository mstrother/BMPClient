using BmpListener.MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    // RFC 4724
    public class CapabilityGracefulRestart : Capability
    {
        public byte Flags { get; private set; }
        public ushort Time { get; private set; }
        public IList<(AddressFamily, SubsequentAddressFamily, byte)> Tuples { get; } = new List<(AddressFamily afi, SubsequentAddressFamily safi, byte flags)> { };

        public override void Decode(ArraySegment<byte> data)
        {
            var restart = EndianBitConverter.Big.ToUInt16(data, 0);
            Flags = (byte)(restart >> 12);
            Time = (ushort)(restart & 0xfff);

            for(int i = 2; i < data.Count;)
            {
                AddressFamily afi = (AddressFamily)EndianBitConverter.Big.ToUInt16(data, i);
                SubsequentAddressFamily safi = (SubsequentAddressFamily)data.ElementAt(i + 1);
                var flags = data.ElementAt(i + 2);
                Tuples.Add((afi, safi, flags));
                i += 4;
            }
        }
    }
}