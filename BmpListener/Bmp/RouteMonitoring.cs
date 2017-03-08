using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public RouteMonitoring(BmpHeader bmpHeader, byte[] data)
            : base(bmpHeader, data)
        {
            Decode(data, Constants.BmpPerPeerHeaderLength);
        }

        public BgpMessage BgpMessage { get; set; }
        
        public void Decode(byte[] data, int offset)
        {
            BgpMessage = (BgpUpdateMessage)BgpMessage.GetBgpMessage(data, offset);
        }
    }
}