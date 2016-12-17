using System;

namespace BMPClient.BGP
{
    public class BGPMessage
    {
        public BGPMessage(BGPHeader header, ImsgBody body)
        {
            Header = header;
            Body = body;
        }

        public BGPHeader Header { get; }
        public ImsgBody Body { get; }

        public static BGPMessage GetBGPMessage(byte[] data)
        {
            var headerData = new ArraySegment<byte>(data, 0, 19);
            var header = new BGPHeader(headerData);
            var msgData = new ArraySegment<byte>(data, 19, data.Length - 19);

            ImsgBody body;

            switch (header.Type)
            {
                case BGP.MessageType.Open:
                    body = new BGPOpen();
                    break;
                case BGP.MessageType.Update:
                    body = new BGPUpdate();
                    break;
                case BGP.MessageType.Notification:
                    throw new NotImplementedException();
                case BGP.MessageType.Keepalive:
                    throw new NotImplementedException();
                case BGP.MessageType.RouteRefresh:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }

            body.DecodeFromBytes(msgData);
            return new BGPMessage(header, body);
        }
    }
}