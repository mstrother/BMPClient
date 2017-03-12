using BmpListener.Bmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpListener.Serialization
{
    public class PeerUpModel
    {
        public PeerUpModel(PeerUpNotification msg)
        {
            LocalAddress = msg.LocalAddress.ToString();
            LocalPort = msg.LocalPort;
            RemotePort = msg.RemotePort;
            SentOpenMessage = new BgpOpenModel(msg.SentOpenMessage);
            ReceivedOpenMessage = new BgpOpenModel(msg.ReceivedOpenMessage);
        }

        public string LocalAddress { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public BgpOpenModel SentOpenMessage { get; set; }
        public BgpOpenModel ReceivedOpenMessage { get; set; }        
    }
}
