﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using BmpListener.Bgp;
using BmpListener.Bmp;
using BmpListener.Serialization.Converters;
using BmpListener.Serialization.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BmpListener.Serialization
{
    public static class BmpSerializer
    {
        static BmpSerializer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>
                {
                    new IPAddressConverter(),
                    new BgpOpenConverter(),
                    new AddressFamilyConverter(),
                    new SubsequentAddressFamilyConverter(),
                    new IPAddrPrefixConverter(),
                    new PathAttributeOriginConverter()
                }
            };
        }

        public static string ToJson(this BmpMessage msg)
        {
            string json;

            switch (msg.BmpHeader.MessageType)
            {
                case BmpMessageType.PeerUp:
                    var peerUpNotificationModel = ConvertToModel(msg as PeerUpNotification);
                    json = JsonConvert.SerializeObject(peerUpNotificationModel);
                    break;
                case BmpMessageType.RouteMonitoring:
                    var routeMonitoringModel = ConvertToModel(msg as RouteMonitoring);
                    json = JsonConvert.SerializeObject(routeMonitoringModel);
                    break;
                default:
                    return null;
            }

            return json;
        }

        private static PeerUpNotificationModel ConvertToModel(PeerUpNotification msg)
        {
            var peer = ConvertToModel(msg.PeerHeader);

            return new PeerUpNotificationModel
            {
                DateTime = msg.PeerHeader.DateTime,
                Peer = peer,
                LocalPort = msg.LocalPort,
                RemotePort = msg.RemotePort
            };
        }

        private static PeerHeaderModel ConvertToModel(PerPeerHeader peerHeader)
        {
            return new PeerHeaderModel
            {
                // type
                Address = peerHeader.PeerAddress,
                Asn = peerHeader.AS,
                Id = peerHeader.PeerId
            };
        }

        private static RouteMonitoringModel ConvertToModel(RouteMonitoring msg)
        {
            var bgpMsg = msg.BgpUpdate;

            if (bgpMsg.Attributes.Count > 4)
            {
            }

            var peerHeaderModel = ConvertToModel(msg.PeerHeader);
            
            var asPath = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.AsPath) as PathAttributeASPath;

            var model = new RouteMonitoringModel
            {
                DateTime = msg.PeerHeader.DateTime,
                Peer = peerHeaderModel,
                Origin = (PathAttributeOrigin)bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.Origin),
                AsPath = asPath?.ASPaths[0].ASNs,
            };
            
            var communities = bgpMsg.Attributes.Where(x => x.AttributeType == PathAttributeType.Community).ToList();
            foreach (var community in communities)
            {
                model.Communities.Add(community.ToString());
            }

            if (bgpMsg.Nlri.Count > 0)
            {
                var nexthop = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.NextHop) as PathAttributeNextHop;
                var announce = ConvertToModel(AddressFamily.IP, SubsequentAddressFamily.Unicast, nexthop?.NextHop, bgpMsg.Nlri);
                model.Announce.Add(announce);
            }

            if (bgpMsg.WithdrawnRoutesLength > 0)
            {
                var withdraw = ConvertToModel(AddressFamily.IP, SubsequentAddressFamily.Unicast, bgpMsg.WithdrawnRoutes);
                model.Withdraw.Add(withdraw);
            }

            var mpReach = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.MpReachNlri) as PathAttributeMPReachNlri;
            if (mpReach?.Nlri.Count > 0)
            {
                var announce = ConvertToModel(mpReach.Afi, mpReach.Safi, mpReach.NextHop, mpReach.Nlri);
                model.Announce.Add(announce);
            }

            var mpUnreach = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.MpUnreachNlri) as PathAttributeMPUnreachNlri;
            if (mpUnreach?.WithdrawnRoutes.Count > 0)
            {
                var withdraw = ConvertToModel(mpUnreach.Afi, mpUnreach.Safi, mpUnreach.WithdrawnRoutes);
                model.Withdraw.Add(withdraw);
            }

            return model;
        }

        private static PrefixAnnounceModel ConvertToModel(AddressFamily afi, SubsequentAddressFamily safi, IPAddress nexthop, IList<IPAddrPrefix> prefixes)
        {
            return new PrefixAnnounceModel
            {
                Afi = afi,
                Safi = safi,
                Nexthop = nexthop,
                Prefixes = prefixes
            };
        }

        private static PrefixWithdrawal ConvertToModel(AddressFamily afi, SubsequentAddressFamily safi,
            IList<IPAddrPrefix> prefixes)
        {
            return new PrefixWithdrawal
            {
                Afi = afi,
                Safi = safi,
                Prefixes = prefixes
            };
        }
    }
}