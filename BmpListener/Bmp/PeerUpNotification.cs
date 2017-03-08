using System;
using System.Linq;
using System.Net;
using BmpListener.Bgp;
using BmpListener.Extensions;

namespace BmpListener.Bmp
{
    public class PeerUpNotification : BmpMessage
    {
        public PeerUpNotification(BmpHeader bmpHeader, byte[] data)
            : base(bmpHeader, data)
        {
            Decode(data, Constants.BmpPerPeerHeaderLength);
        }

        public IPAddress LocalAddress { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public BgpMessage SentOpenMessage { get; set; }
        public BgpMessage ReceivedOpenMessage { get; set; }

        public void Decode(byte[] data, int offset)
        {
            // For IPv4 peers, the most significant bit of PeerHeader.Flags will be set to 0.
            if (((PeerHeader.Flags & (1 << 7)) != 0))
            {
                var ipBytes = new byte[16];
                Array.Copy(data, offset, ipBytes, 0, 16);
                LocalAddress = new IPAddress(ipBytes);
            }
            else
            {
                var ipBytes = new byte[4];
                Array.Copy(data, offset + 12, ipBytes, 0, 4);
                LocalAddress = new IPAddress(ipBytes);
            }

            Array.Reverse(data, offset + 16, 2);
            LocalPort = BitConverter.ToUInt16(data, offset + 16);

            Array.Reverse(data, offset + 18, 2);
            RemotePort = BitConverter.ToUInt16(data, offset + 18);

            offset += 20;
            SentOpenMessage = BgpMessage.GetBgpMessage(data, offset);
            offset += SentOpenMessage.Header.Length;
            ReceivedOpenMessage = BgpMessage.GetBgpMessage(data, offset);
        }
    }
}