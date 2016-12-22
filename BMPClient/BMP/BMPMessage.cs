namespace BmpListener.BMP
{
    public class BMPMessage
    {
        public enum BMPMessageType : byte
        {
            RouteMonitoring,
            StatisticsReport,
            PeerDown,
            PeerUp,
            Initiation,
            Termination,
            RouteMirroring
        }

        public BMPMessage(Header header)
        {
            Header = header;
        }

        public Header Header { get; set; }
        public PeerHeader PeerHeader { get; set; }
        public IBMPBody Body { get; set; }
    }
}