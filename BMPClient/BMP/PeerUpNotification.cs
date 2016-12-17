using System;
using System.Net;
using BMPClient.BGP;

namespace BMPClient.BMP
{
    public class PeerUpNotification : IBMPBody
    {
        public PeerUpNotification(BMPMessage message, byte[] data)
        {
            ParseBody(message, data);
        }

        public IPAddress LocalAddress { get; set; }
        public ushort LocalPort { get; set; }
        public ushort RemotePort { get; set; }
        public BGPMessage SentOpenMessage { get; set; }
        public BGPMessage ReceivedOpenMessage { get; set; }

        public void ParseBody(BMPMessage message, byte[] data)
        {
            if ((message.PeerHeader.Flags & (1 << 7)) != 0)
            {
                var ipBytes = new byte[16];
                Buffer.BlockCopy(data, 0, ipBytes, 0, 16);
                LocalAddress = new IPAddress(ipBytes);
            }

            LocalPort = data.ToUInt16(16);
            RemotePort = data.ToUInt16(18);

            var bgpData = new byte[data.Length - 20];
            Buffer.BlockCopy(data, 20, bgpData, 0, data.Length - 20);

            var bgpMsg = BGPMessage.GetBGPMessage(bgpData);
        }
    }
}