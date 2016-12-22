using System;

namespace BmpListener.BGP
{
    public class BGPMsg
    {
        public BGPMsg(BGPHeader header, ImsgBody body)
        {
            Header = header;
            Body = body;
        }

        public BGPHeader Header { get; }
        public ImsgBody Body { get; }

        public static BGPMsg GetBGPMessage(byte[] data)
        {
            var headerData = new ArraySegment<byte>(data, 0, 19);
            var header = new BGPHeader(headerData);
            var msgData = new ArraySegment<byte>(data, 19, data.Length - 19);

            ImsgBody body;

            switch (header.Type)
            {
                case BGP.MessageType.Open:
                    body = new BGPOpenMsg();
                    break;
                case BGP.MessageType.Update:
                    body = new BGPUpdateMsg();
                    break;
                case BGP.MessageType.Notification:
                    body = new BGPNotification();
                    break;
                case BGP.MessageType.Keepalive:
                    body = new BGPKeepAliveMsg();
                    break;
                case BGP.MessageType.RouteRefresh:
                    body = new BGPRouteMsg();
                    break;
                default:
                    throw new NotImplementedException();
            }

            body.DecodeFromBytes(msgData);
            return new BGPMsg(header, body);
        }
    }
}