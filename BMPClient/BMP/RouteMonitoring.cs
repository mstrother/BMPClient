using BMPClient.BGP;

namespace BMPClient.BMP
{
    public class RouteMonitoring : IBMPBody
    {
        public RouteMonitoring(BMPMessage message, byte[] data)
        {
            ParseBody(message, data);
        }

        public BGPMsg BGPUpdate { get; set; }

        public void ParseBody(BMPMessage message, byte[] data)
        {
            BGPUpdate = BGPMsg.GetBGPMessage(data);
        }
    }
}