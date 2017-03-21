using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public RouteMonitoring(BmpHeader bmpHeader, byte[] data)
            : base(bmpHeader, data)
        {
            var offset = Constants.BmpCommonHeaderLength + Constants.BmpPerPeerHeaderLength;
            Decode(data, offset);
        }

        public BgpMessage BgpMessage { get; set; }

        public void Decode(byte[] data, int offset)
        {
            BgpMessage = (BgpUpdateMessage)BgpMessage.GetBgpMessage(data, offset);
        }
    }
}