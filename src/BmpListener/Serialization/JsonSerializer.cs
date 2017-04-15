using BmpListener.Bgp;
using BmpListener.Bmp;
using BmpListener.Serialization.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Serialization
{
    public static class JsonSerializer
    {
        private static Dictionary<AddressFamily, string> afiStringMap = new Dictionary<AddressFamily, string>()
        {
            {AddressFamily.IP, "ip" },
            {AddressFamily.IP6, "ipv6"  }
        };

        private static Dictionary<SubsequentAddressFamily, string> safiStringMap = new Dictionary<SubsequentAddressFamily, string>()
        {
            {SubsequentAddressFamily.Multicast, "multicast" },
            {SubsequentAddressFamily.Unicast, "unicast"  }
        };

        static JsonSerializer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public static string ToJson(this BmpMessage msg)
        {
            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessageType.PeerUp):
                    var peerUpmodel = CreateModel((PeerUpNotification)msg);
                    return JsonConvert.SerializeObject(peerUpmodel);
                case (BmpMessageType.RouteMonitoring):
                    var updateModel = CreateModel((RouteMonitoring)msg);
                    return JsonConvert.SerializeObject(updateModel);
                default:
                    return null;
            }
        }

        public static PeerHeaderModel CreateModel(PerPeerHeader peerHeader)
        {
            return new PeerHeaderModel
            {
                Type = "global",
                Address = peerHeader.PeerAddress.ToString(),
                Asn = peerHeader.AS,
                Id = peerHeader.PeerId.ToString(),
                Time = peerHeader.DateTime
            };
        }

        public static PeerUpModel CreateModel(PeerUpNotification bmpMsg)
        {
            return new PeerUpModel
            {
                Peer = CreateModel(bmpMsg.PeerHeader),
                LocalPort = bmpMsg.LocalPort,
                RemotePort = bmpMsg.RemotePort
            };
        }

        private static UpdateModel CreateModel(RouteMonitoring bmpMsg)
        {
            var bgpMsg = bmpMsg.BgpMessage as BgpUpdateMessage;

            var model = new UpdateModel();
            model.Peer = CreateModel(bmpMsg.PeerHeader);

            // End-of-RIB
            if (bgpMsg.Header.Length == Constants.BgpHeaderLength)
            {
                return model;
            }

            var origin = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.ORIGIN);
            model.Origin = ((PathAttributeOrigin)origin).Origin.ToString();

            var asPath = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.AS_PATH) as PathAttributeASPath;
            if (asPath != null)
            {
                model.AsPath = new List<int>();
                for (int i = 0; i < asPath.ASPaths[0].ASNs.Count; i++)
                {
                    model.AsPath.Add(asPath.ASPaths[0].ASNs[i]);
                }
            }

            if (bgpMsg.WithdrawnRoutesLength > 0)
            {
                model.Withdraw = new List<WithdrawModel>();

                var withdrawModel = new WithdrawModel
                {
                    Afi = afiStringMap[AddressFamily.IP],
                    Safi = safiStringMap[SubsequentAddressFamily.Unicast],
                    Prefixes = new List<string>()
                };

                for (int i = 0; i < bgpMsg.WithdrawnRoutes.Count; i++)
                {
                    var prefix = bgpMsg.WithdrawnRoutes[i].ToString();
                    withdrawModel.Prefixes.Add(prefix);
                }

                model.Withdraw.Add(withdrawModel);
            }

            if (bgpMsg.Nlri?.Count > 0)
            {
                model.Announce = new List<AnnounceModel>();

                var nexthop = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.NEXT_HOP);
                var announceModel = new AnnounceModel
                {
                    Afi = afiStringMap[AddressFamily.IP],
                    Safi = safiStringMap[SubsequentAddressFamily.Unicast],
                    Nexthop = ((PathAttributeNextHop)nexthop).NextHop.ToString(),
                    Prefixes = new List<string>()
                };

                for (int i = 0; i < bgpMsg.Nlri.Count; i++)
                {
                    var prefix = bgpMsg.Nlri[i].ToString();
                    announceModel.Prefixes.Add(prefix);
                }

                model.Announce.Add(announceModel);
            }

            var mpReach = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.MP_REACH_NLRI) as PathAttributeMPReachNLRI;
            if (mpReach != null)
            {
                model.Announce = new List<AnnounceModel>();
                
                var announceModel = new AnnounceModel
                {
                    Afi = afiStringMap[mpReach.AFI],
                    Safi = safiStringMap[mpReach.SAFI],
                    Nexthop = mpReach.NextHop.ToString(),
                    Prefixes = new List<string>()
                };

                for (int i = 0; i < mpReach.NLRI.Count; i++)
                {
                    var prefix = bgpMsg.Nlri[i].ToString();
                    announceModel.Prefixes.Add(prefix);
                }

                model.Announce.Add(announceModel);
            }

            return model;
        }            
    }
}
