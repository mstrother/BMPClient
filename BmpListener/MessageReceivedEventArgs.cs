using BmpListener.Bmp;
using System;

namespace BmpListener
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(BmpMessage msg)
        {
            BmpMessage = msg;
        }

        public BmpMessage BmpMessage { get; }
    }
}
