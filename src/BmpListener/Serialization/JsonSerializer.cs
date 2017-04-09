using BmpListener.Bgp;
using BmpListener.Bmp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BmpListener.Serialization
{
    public static class JsonSerializer
    {
        public static string ToJson(this BmpMessage msg)
        {
            var sw = new StringWriter();

            using (var writer = new JsonTextWriter(sw))
            {
                if (msg.PeerHeader != null)
                {
                    writer.WriteStartObject();

                    var peerHeader = msg.PeerHeader.ToJson();
                    writer.WriteRawAsync(peerHeader);

                    if (msg.BmpHeader.MessageType == BmpMessageType.RouteMonitoring)
                    {
                        writer.WritePropertyNameAsync("update");
                        writer.WriteStartObject();

                        var bgpMsg = ((RouteMonitoring)msg).BgpMessage;
                        if (bgpMsg.Header.Type == BgpMessageType.Update)
                        {
                            var attributes = ((BgpUpdateMessage)bgpMsg).Attributes;
                            foreach (var attribute in attributes)
                            {
                                switch (attribute.AttributeType)
                                {
                                    case PathAttributeType.ORIGIN:
                                        writer.WritePropertyNameAsync("origin");
                                        if (((PathAttributeOrigin)attribute).Origin == PathAttributeOrigin.Type.EGP)
                                        {
                                            writer.WriteValueAsync("egp");
                                        }
                                        else if (((PathAttributeOrigin)attribute).Origin == PathAttributeOrigin.Type.IGP)
                                        {
                                            writer.WriteValueAsync("igp");
                                        }
                                        else
                                        {
                                            writer.WriteValueAsync("incomplete");
                                        }
                                        break;
                                    case PathAttributeType.AS_PATH:
                                        var asPath = (PathAttributeASPath)attribute;
                                        var path = asPath.ASPaths.FirstOrDefault();
                                        writer.WritePropertyNameAsync("as-path");
                                        writer.WriteStartArray();
                                        foreach (var asn in path.ASNs)
                                        {
                                            writer.WriteValueAsync(asn);
                                        }
                                        writer.WriteEndArray();
                                        break;
                                    case PathAttributeType.NEXT_HOP:
                                        writer.WritePropertyNameAsync("nexthop");
                                        writer.WriteValueAsync(((PathAttributeNextHop)attribute).NextHop.ToString());
                                        break;
                                    case PathAttributeType.MULTI_EXIT_DISC:
                                        break;
                                    case PathAttributeType.LOCAL_PREF:
                                        break;
                                    case PathAttributeType.ATOMIC_AGGREGATE:
                                        break;
                                    case PathAttributeType.AGGREGATOR:
                                        break;
                                    case PathAttributeType.COMMUNITY:
                                        break;
                                    case PathAttributeType.ORIGINATOR_ID:
                                        break;
                                    case PathAttributeType.CLUSTER_LIST:
                                        break;
                                    case PathAttributeType.MP_REACH_NLRI:
                                        break;
                                    case PathAttributeType.MP_UNREACH_NLRI:
                                        break;
                                    case PathAttributeType.EXTENDED_COMMUNITIES:
                                        break;
                                    case PathAttributeType.AS4_PATH:
                                        break;
                                    case PathAttributeType.AS4_AGGREGATOR:
                                        break;
                                    case PathAttributeType.PMSI_TUNNEL:
                                        break;
                                    case PathAttributeType.TUNNEL_ENCAP:
                                        break;
                                    case PathAttributeType.AIGP:
                                        break;
                                    case PathAttributeType.LARGE_COMMUNITY:
                                        break;
                                }                                
                            }

                            var nlri = ((BgpUpdateMessage)bgpMsg).Nlri;
                            if (nlri.Count > 0)
                            {
                                writer.WritePropertyNameAsync("announce");
                                writer.WriteStartArray();
                                foreach (var route in nlri)
                                {
                                    writer.WriteValueAsync(route.ToString());
                                }
                                writer.WriteEndArray();
                            }

                            var withdrawnRoutes = ((BgpUpdateMessage)bgpMsg).WithdrawnRoutes;
                            if (withdrawnRoutes.Count > 0)
                            {
                                writer.WritePropertyNameAsync("withdraw");
                                writer.WriteStartArray();
                                foreach (var route in withdrawnRoutes)
                                {
                                    writer.WriteValueAsync(route.ToString());
                                }
                                writer.WriteEndArray();
                            }
                        }

                        writer.WriteEndObject();
                    }

                    writer.WriteEndObject();

                    return sw.ToString();
                }
            }
            return null;
        }

        public static string ToJson(this PerPeerHeader peerHeader)
        {
            var sw = new StringWriter();

            using (var writer = new JsonTextWriter(sw))
            {
                writer.WritePropertyNameAsync("peer");
                writer.WriteStartObject();

                writer.WritePropertyNameAsync("type");
                writer.WriteValueAsync("global");

                writer.WritePropertyNameAsync("address");
                writer.WriteValueAsync(peerHeader.PeerAddress.ToString());

                writer.WritePropertyNameAsync("asn");
                writer.WriteValueAsync(peerHeader.AS);

                writer.WritePropertyNameAsync("id");
                writer.WriteValueAsync(peerHeader.PeerId.ToString());

                writer.WritePropertyNameAsync("time");
                writer.WriteValueAsync(peerHeader.DateTime.ToString("o"));

                writer.WriteEndObject();

                return sw.ToString();
            }
        }
    }
}
