using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class PeerDownNotification : IBMPBody
    {
        public short Reason { get; private set; }
        public BgpMessage BGPNotification { get; set; }

        public void ParseBody(BmpMessage message, byte[] messageBytes)
        {
            Reason = messageBytes[0];
        }
    }
}