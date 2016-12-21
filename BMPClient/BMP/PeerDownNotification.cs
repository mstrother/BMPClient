using BMPClient.BGP;

namespace BMPClient.BMP
{
    public class PeerDownNotification : IBMPBody
    {
        public short Reason { get; private set; }
        public BGPMsg BGPNotification { get; set; }
        private byte[] Data { get; set; }

        public void ParseBody(BMPMessage message, byte[] messageBytes)
        {
            Reason = messageBytes[0];
        }
    }
}