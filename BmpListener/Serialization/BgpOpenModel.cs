using BmpListener.Bgp;

namespace BmpListener.Serialization
{
    public class BgpOpenModel
    {
        public BgpOpenModel(BgpOpenMessage msg)
        {
            Version = msg.Version;
            AS = msg.MyAS;
            HoldTime = msg.HoldTime;
        }

        public byte Version { get; }
        public int AS { get; }
        public int HoldTime { get; }
    }
}
