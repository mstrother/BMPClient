using System;
using System.Net;
using BmpListener.Bgp;
using System.Linq;
using BmpListener.MiscUtil.Conversion;

namespace BmpListener.Bmp
{
    public class PeerUpNotification : BmpMessage
    {
        public IPAddress LocalAddress { get; private set; }
        public int LocalPort { get; private set; }
        public int RemotePort { get; private set; }
        public BgpOpenMessage SentOpenMessage { get; private set; }
        public BgpOpenMessage ReceivedOpenMessage { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            if (((PeerHeader.Flags & (1 << 7)) != 0))
            {
                var ipBytes = new byte[16];
                Array.Copy(data, offset, ipBytes, 0, 4);
                LocalAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data, offset + 12, ipBytes, 0, 4);
                LocalAddress = new IPAddress(ipBytes);
            }

            LocalPort = EndianBitConverter.Big.ToUInt16(data, 16);
            RemotePort = EndianBitConverter.Big.ToUInt16(data, 18);

            offset += 20;
            SentOpenMessage = BgpMessage.DecodeMessage(data, offset) as BgpOpenMessage;

            offset += SentOpenMessage.Header.Length;
            ReceivedOpenMessage = BgpMessage.DecodeMessage(data, offset) as BgpOpenMessage;
        }
    }
}