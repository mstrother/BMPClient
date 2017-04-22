using BmpListener.MiscUtil.Conversion;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    // RFC 4724
    public class CapabilityGracefulRestart : Capability
    {
        public byte Flags { get; private set; }
        public ushort Time { get; private set; }
        public IList<(AddressFamily, SubsequentAddressFamily, byte)> Tuples { get; } = new List<(AddressFamily afi, SubsequentAddressFamily safi, byte flags)> { };

        public override void Decode(byte[] data, int offset)
        {
            var restart = EndianBitConverter.Big.ToUInt16(data, offset);
            Flags = (byte)(restart >> 12);
            Time = (ushort)(restart & 0xfff);

            for (int i = 2; i < Length;)
            {
                AddressFamily afi = (AddressFamily)EndianBitConverter.Big.ToUInt16(data, offset + i);
                SubsequentAddressFamily safi = (SubsequentAddressFamily)data[offset + i + 1];
                var flags = data[offset + i + 2];
                Tuples.Add((afi, safi, flags));
                offset += 4;
                i += 4;
            }
        }
    }
}