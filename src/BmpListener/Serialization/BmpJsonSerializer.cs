using BmpListener.Bgp;
using BmpListener.Bmp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BmpListener.Serialization
{
    public static class BmpJsonSerializer
    {
        public static string Serialize(BmpMessage msg)
        {
            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessageType.Initiation):
                    return ((BmpInitiation)msg).ToJson();
                case (BmpMessageType.PeerUp):
                    return ((PeerUpNotification)msg).ToJson();
                case (BmpMessageType.PeerDown):
                    return null;
                case (BmpMessageType.RouteMonitoring):
                    return ((RouteMonitoring)msg).ToJson();
                case (BmpMessageType.Termination):
                    return null;
                default:
                    return null;
            }
        }

        public static string ToJson(this BmpInitiation msg)
        {
            using (StringWriter sw = new StringWriter())
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
                return sw.ToString();
            }
        }

        public static string ToJson(this PeerUpNotification msg)
        {
            using (StringWriter sw = new StringWriter())
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("LocalAddress");
                writer.WriteValue(msg.LocalAddress.ToString());

                writer.WritePropertyName("LocalPort");
                writer.WriteValue(msg.LocalPort);

                writer.WritePropertyName("RemotePort");
                writer.WriteValue(msg.RemotePort);

                writer.WritePropertyName("SentOpenMessage");
                writer.WriteStartObject();
                writer.WritePropertyName("Version");
                writer.WriteValue(msg.SentOpenMessage.Version);
                writer.WritePropertyName("ASN");
                writer.WriteValue(msg.SentOpenMessage.MyAS);
                writer.WritePropertyName("HoldTime");
                writer.WriteValue(msg.SentOpenMessage.HoldTime);
                writer.WritePropertyName("Capabilities");
                writer.WriteStartArray();
                writer.WriteEndArray();
                writer.WriteEndObject();

                writer.WritePropertyName("ReceivedOpenMessage");
                writer.WriteStartObject();
                writer.WritePropertyName("Version");
                writer.WriteValue(msg.ReceivedOpenMessage.Version);
                writer.WritePropertyName("ASN");
                writer.WriteValue(msg.ReceivedOpenMessage.MyAS);
                writer.WritePropertyName("HoldTime");
                writer.WriteValue(msg.ReceivedOpenMessage.HoldTime);
                writer.WritePropertyName("Capabilities");
                writer.WriteStartArray();
                writer.WriteEndArray();
                writer.WriteEndObject();

                writer.WriteEndObject();

                return sw.ToString();
            }
        }

        public static string ToJson(this RouteMonitoring msg)
        {
            var model = new RouteMonitoringModel(msg);

            using (StringWriter sw = new StringWriter())
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Peer");
                writer.WriteStartObject();
                writer.WritePropertyName("IP");
                writer.WriteValue(msg.PeerHeader.PeerAddress.ToString());
                writer.WritePropertyName("ASN");
                writer.WriteValue(msg.PeerHeader.AS);
                writer.WritePropertyName("PostPolicy");
                writer.WriteValue(msg.PeerHeader.IsPostPolicy);
                writer.WriteEndObject();

                if (model.ASPath?.Count > 0)
                {
                    writer.WritePropertyName("ASPath");
                    writer.WriteStartArray();
                    for (var i = 0; i < model.ASPath.Count; i++)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName(model.ASPath[i].SegmentType.ToString());
                        writer.WriteStartArray();
                        for (var j = 0; j < model.ASPath[i].ASNs.Count; j++)
                        {
                            writer.WriteValue(model.ASPath[i].ASNs[j]);
                        }
                        writer.WriteEndArray();
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                }

                writer.WritePropertyName("Announce");
                writer.WriteStartObject();
                writer.WritePropertyName(model.Announce.AddressFamily);
                writer.WriteStartObject();
                writer.WritePropertyName("Nexthop");
                writer.WriteValue(model.Announce.NextHop);
                writer.WritePropertyName("Routes");
                writer.WriteStartArray();
                for (int i = 0; i < model.Announce.Routes.Count; i++)
                {
                    writer.WriteValue(model.Announce.Routes[i]);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
                writer.WriteEndObject();

                if (model.Withdraw.Count > 0)
                {
                    writer.WritePropertyName("Withdraw");
                    writer.WriteStartObject();
                    writer.WritePropertyName("Routes");
                    writer.WriteStartArray();
                    for (int i = 0; i < model.Withdraw.Count; i++)
                    {
                        writer.WriteValue(model.Withdraw[i]);
                    }
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();

                return sw.ToString();
            }
        }
    }
}
