using BmpListener.Bgp;
using BmpListener.Bmp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;

namespace BmpListener.Json
{
    public class JsonSerializer
    {
        public JsonSerializer()
        {
        }

        public string Version { get; private set; }
        public string Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public BmpPeerHeader Peer { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PathAttributes Attributes { get; private set; }
        public Dictionary<string, dynamic> Announce { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, dynamic> Withdraw { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PeerUpNotification PeerUp { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PeerDownNotification PeerDown { get; private set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BmpInitiation Initiation { get; private set; }

        public string ToJson(BmpMessage msg)
        {
            Serialize(msg);
            return JsonConvert.SerializeObject(this);
        }

        public void Serialize(BmpMessage msg)
        {
            Version = "0.0.1";
            Id = Guid.NewGuid().ToString("N");
            if (msg.PeerHeader != null)
            {
                DateTime = msg.PeerHeader.DateTime;
                Peer = msg.PeerHeader;
            }
            else
            {
                DateTime = DateTime.UtcNow;
            }

            switch (msg.BmpHeader.Type)
            {
                case (Bmp.MessageType.Initiation):
                    Initiation = Initiation;
                    break;
                case (Bmp.MessageType.PeerUp):
                    PeerUp = ((PeerUpNotification)msg);
                    break;
                case (Bmp.MessageType.PeerDown):
                    PeerDown = ((PeerDownNotification)msg);
                    break;
                case (Bmp.MessageType.RouteMonitoring):
                    var bgpUpdateMsg = ((RouteMonitoring)msg).Update;
                    Serialize(bgpUpdateMsg);
                    break;
                case (Bmp.MessageType.Termination):
                    break;
                default:
                    break;
            }
        }

        public void Serialize(BgpUpdateMessage bgpMsg)
        {
            Attributes = new PathAttributes();
            Attributes.Origin = bgpMsg.Attributes.OfType<PathAttributeOrigin>()
                .FirstOrDefault()?.Origin;
            Attributes.ASPath = bgpMsg.Attributes.OfType<PathAttributeASPath>()
                .FirstOrDefault()?.ASPaths.FirstOrDefault()?.ASNs;
            Attributes.AtomicAggregate =
                bgpMsg.Attributes.OfType<PathAttrAtomicAggregate>().Any();

            PathAttributeAggregator aggregator =
                bgpMsg.Attributes.OfType<PathAttributeAggregator>().FirstOrDefault();
            if (aggregator != null)
            {
                Attributes.Aggregator = new Aggregator
                {
                    AS = aggregator.AS,
                    IPAddress = aggregator.IPAddress
                };
            }

            if (bgpMsg.Attributes.OfType<PathAttributeMPReachNLRI>().Any())
            {
                var nlri = bgpMsg.Attributes.OfType<PathAttributeMPReachNLRI>().First();
                dynamic announce = new ExpandoObject();
                announce.Nexthop = nlri.NextHop;
                announce.Routes = nlri.Value;
                Announce = new Dictionary<string, dynamic>();
                var afi = nlri.AFI.ToString().ToLower();
                var safi = nlri.SAFI.ToString().ToLower();
                Announce.Add($"{afi} {safi}", announce);
            }

            if (bgpMsg.Attributes.OfType<PathAttributeMPUnreachNLRI>().Any())
            {
                var nlri = bgpMsg.Attributes.OfType<PathAttributeMPUnreachNLRI>().First();
                dynamic withdraw = new ExpandoObject();
                withdraw.Routes = nlri.Value;
                Withdraw = new Dictionary<string, dynamic>();
                var afi = nlri.AFI.ToString().ToLower();
                var safi = nlri.SAFI.ToString().ToLower();
                Withdraw.Add($"{afi} {safi}", withdraw);                               
            }
        }

        public class PathAttributes
        {
            public Origin? Origin { get; set; }
            [JsonProperty(PropertyName = "asPath")]
            public int[] ASPath { get; set; }
            public bool AtomicAggregate { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Aggregator Aggregator { get; set; }           
        }

        public class Aggregator
        {
            [JsonProperty(PropertyName = "as")]
            public int AS { get; set; }
            [JsonProperty(PropertyName = "ip")]
            public IPAddress IPAddress { get; set; }
        }
    }
}
