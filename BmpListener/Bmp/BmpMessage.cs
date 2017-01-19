using BmpListener.Bgp;
using Newtonsoft.Json;

namespace BmpListener.Bmp
{
    public class BmpMessage
    {
        public BmpMessage(BmpHeader bmpHeader)
        {
            BmpHeader = bmpHeader;
        }

        public BmpHeader BmpHeader { get; set; }
        public PeerHeader PeerHeader { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBMPBody Body { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BgpUpdateMessage Update { get; set; }
    }
}