using System;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        protected BmpMessage(byte[] data)
        {
            BmpHeader = new BmpHeader(data);
            if (BmpHeader?.MessageType != BmpMessageType.Initiation)
            {
                PeerHeader = new PerPeerHeader(data, Constants.BmpCommonHeaderLength);
            }
        }

        protected BmpMessage(BmpHeader header)
        {
            BmpHeader = header;
        }

        public BmpHeader BmpHeader { get; }
        public PerPeerHeader PeerHeader { get; }
    }
}