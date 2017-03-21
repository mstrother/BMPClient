using System;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        protected BmpMessage(BmpHeader header, byte[] data)
        {
            BmpHeader = header;
            PeerHeader = new PerPeerHeader(data, Constants.BmpCommonHeaderLength);
        }

        protected BmpMessage(BmpHeader header)
        {
            BmpHeader = header;
        }
                
        public BmpHeader BmpHeader { get; }
        public PerPeerHeader PeerHeader { get; }
    }
}