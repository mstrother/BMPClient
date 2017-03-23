using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public RouteMonitoring(byte[] data)
            : base(data)
        {
            var offset = Constants.BmpCommonHeaderLength + Constants.BmpPerPeerHeaderLength;
            Decode(data, offset);
        }

        public BgpMessage BgpMessage { get; set; }

        public void Decode(byte[] data, int offset)
        {
            BgpMessage = BgpMessage.Create(data, offset);
        }
    }
}