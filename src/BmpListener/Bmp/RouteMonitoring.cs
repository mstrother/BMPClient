using BmpListener.Bgp;

namespace BmpListener.Bmp
{
    public class RouteMonitoring : BmpMessage
    {
        public BgpMessage BgpMessage { get; set; }

        public override void Decode(byte[] data, int offset)
        {
            BgpMessage = BgpMessage.ParseMessage(data, offset);
        }
    }
}