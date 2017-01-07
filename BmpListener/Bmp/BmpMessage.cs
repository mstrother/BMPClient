namespace BmpListener.Bmp
{
    public class BmpMessage
    {
  

        public BmpMessage(BmpHeader bmpHeader)
        {
            BmpHeader = bmpHeader;
        }

        public BmpHeader BmpHeader { get; set; }
        public PeerHeader PeerHeader { get; set; }
        public IBMPBody Body { get; set; }
    }
}