using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public BgpUpdateMessage BgpUpdate { get; set; }

        public override void Decode(byte[] data, int offset)
        {
            BgpUpdate = BgpMessage.Create(data, offset) as BgpUpdateMessage;
        }
    }
}