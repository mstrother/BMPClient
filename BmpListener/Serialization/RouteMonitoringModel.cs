using BmpListener.Bgp;
using BmpListener.Bmp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Serialization
{
    public class RouteMonitoringModel
    {
        public RouteMonitoringModel(RouteMonitoring msg)
        {
            BgpUpdateMessage bgpMsg;

            if (msg.BgpMessage.Header.Type == BgpMessageType.Update)
            {
                bgpMsg = (BgpUpdateMessage)msg.BgpMessage;
            }
            else
            {
                return;
            }

            ParseAttributes(bgpMsg.Attributes);

            if (Announce == null)
            {
                var nextHop = bgpMsg.Attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.NEXT_HOP);
                Announce = new AnnounceModel(bgpMsg.Nlri, (PathAttributeNextHop)nextHop);
            }

            Withdraw = new List<IPAddrPrefix>();

            if (bgpMsg.WithdrawnRoutes?.Count > 0)
            {
                Withdraw = new List<IPAddrPrefix>();
                for (int i = 0; i < bgpMsg.WithdrawnRoutes.Count; i++)
                {
                    bgpMsg.WithdrawnRoutes[i].ToString();
                }
            }
        }

        public IList<ASPathSegment> ASPath { get; private set; }
        public PathAttributeOrigin.Type? Origin { get; private set; }
        public uint? Communities { get; private set; }
        public AnnounceModel Announce { get; private set; }
        public IList<IPAddrPrefix> Withdraw { get; }

        public void ParseAttributes(List<PathAttribute> attributes)
        {
            var asPath = attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.AS_PATH);
            var origin = attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.ORIGIN);
            var communities = attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.COMMUNITY);
            var mpReach = attributes.FirstOrDefault(x => x.AttributeType == PathAttributeType.MP_REACH_NLRI);

            ASPath = ((PathAttributeASPath)asPath)?.ASPaths;
            Origin = ((PathAttributeOrigin)origin)?.Origin;
            Communities = ((PathAttributeCommunity)communities)?.Community;

            if (mpReach != null)
            {
                Announce = new AnnounceModel((PathAttributeMPReachNLRI)mpReach);
            }
        }
    }
}
