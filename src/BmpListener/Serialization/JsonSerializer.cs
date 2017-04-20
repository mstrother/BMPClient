using BmpListener.Bgp;
using BmpListener.Bmp;
using BmpListener.Serialization.Converters;
using BmpListener.Serialization.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BmpListener.Serialization
{
    public static class JsonSerializer
    {
        static JsonSerializer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new IPAddressConverter(), new BgpOpenConverter(), new BgpUpdateConverter(), new AddressFamilyConverter(), new SubsequentAddressFamilyConverter(), new IPAddrPrefixConverter() }
            };
        }

        public static string ToJson(this IBmpMessage msg)
        {
            JObject jObject = null;

            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessageType.PeerUp):
                    var model = ConvertToModel((PeerUpNotification)msg);
                    jObject = JObject.FromObject(model);
                    break;
                case (BmpMessageType.RouteMonitoring):
                    jObject = ToJObject((RouteMonitoring)msg);
                    break;
                default:
                    return null;
            }

            var json = JsonConvert.SerializeObject(jObject);
            return json;
        }

        private static PeerUpNotificationModel ConvertToModel(PeerUpNotification msg)
        {
            var peer = ConvertToModel(msg.PeerHeader);

            return new PeerUpNotificationModel
            {
                Peer = peer,
                LocalPort = msg.LocalPort,
                RemotePort = msg.RemotePort,
            };
        }

        private static PeerHeaderModel ConvertToModel(PerPeerHeader peerHeader)
        {
            return new PeerHeaderModel
            {
                // type
                Address = peerHeader.PeerAddress,
                Asn = peerHeader.AS,
                Id = peerHeader.PeerId,
                Time = peerHeader.DateTime
            };
        }

        private static JObject ToJObject(RouteMonitoring msg)
        {
            var peerHeaderModel = ConvertToModel(msg.PeerHeader);

            var jObject = new JObject
            {
                { "bmpMsgLength", msg.BmpHeader.MessageLength },
                { "bgpMsgLength", msg.Header.Length },
                { "peer", JObject.FromObject(peerHeaderModel) }
            };

            if (msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.ORIGIN) is PathAttributeOrigin origin)
            {
                switch (origin.Origin)
                {
                    case PathAttributeOrigin.Type.IGP:
                        jObject.Add("origin", "igp");
                        break;
                    case PathAttributeOrigin.Type.EGP:
                        jObject.Add("origin", "egp");
                        break;
                    case PathAttributeOrigin.Type.Incomplete:
                        jObject.Add("origin", "incomplete");
                        break;
                }
            }

            if (msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.MULTI_EXIT_DISC) is PathAttributeMultiExitDisc med)
            {
                jObject.Add("med", med.Metric);
            }

            if (msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.COMMUNITY) is PathAttributeCommunity community)
            {
                jObject.Add("community", community.ToString());
            }

            var asPath = msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.AS_PATH) as PathAttributeASPath;
            var asns = asPath?.ASPaths[0].ASNs;
            if (asns?.Count > 0)
            {
                var jarrayObj = new JArray(asns);
                jObject.Add("asPath", jarrayObj);
            }

            var announceObj = new JArray();
            var withdrawObj = new JArray();

            if (msg.Nlri.Count > 0)
            {
                var nexthop = msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.NEXT_HOP) as PathAttributeNextHop;
                var announce = ToJObject(AddressFamily.IP, SubsequentAddressFamily.Unicast, nexthop.NextHop, msg.Nlri);
                announceObj.Add(announce);
            }

            if (msg.WithdrawnRoutesLength > 0)
            {
                var withdraw = ToJObject(AddressFamily.IP, SubsequentAddressFamily.Unicast, msg.WithdrawnRoutes);
                withdrawObj.Add(withdraw);
            }

            var mpReach = msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.MP_REACH_NLRI) as PathAttributeMPReachNlri;
            if (mpReach?.NLRI.Count > 0)
            {
                var nexthop = msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.NEXT_HOP) as PathAttributeNextHop;
                var announce = ToJObject(mpReach.Afi, mpReach.Safi, mpReach.NextHop, mpReach.NLRI);
                announceObj.Add(announce);
            }

            var mpUnreach = msg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.MP_UNREACH_NLRI) as PathAttributeMPUnreachNlri;
            if (mpUnreach?.WithdrawnRoutes.Count > 0)
            {
                var withdraw = ToJObject(mpUnreach.Afi, mpUnreach.Safi, mpUnreach.WithdrawnRoutes);
                withdrawObj.Add(withdraw);
            }

            if (announceObj.Count > 0)
            {
                jObject.Add("announce", announceObj);
            }

            if (withdrawObj.Count > 0)
            {
                jObject.Add("withdraw", withdrawObj);
            }

            return jObject;
        }

        private static JObject ToJObject(AddressFamily afi, SubsequentAddressFamily safi, IPAddress nexthop, IList<IPAddrPrefix> prefixes)
        {
            var model = new PrefixAnnounceModel
            {
                Afi = afi,
                Safi = safi,
                Nexthop = nexthop,
                Prefixes = prefixes
            };

            var jObject = JObject.FromObject(model);
            return jObject;
        }

        private static JObject ToJObject(AddressFamily afi, SubsequentAddressFamily safi, IList<IPAddrPrefix> prefixes)
        {
            var model = new PrefixWithdrawal
            {
                Afi = afi,
                Safi = safi,
                Prefixes = prefixes
            };

            var jObject = JObject.FromObject(model);
            return jObject;
        }
    }
}
