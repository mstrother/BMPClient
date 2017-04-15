using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public enum Type
        {
            IGP,
            EGP,
            Incomplete
        }

        public Type Origin { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Origin = (Type)data[offset];
        }

        public override string ToString()
        {
            switch (Origin)
            {
                case (Type.IGP):
                    return "igp";
                case (Type.EGP):
                    return "egp";
                case (Type.Incomplete):
                    return "incomplete";
                default:
                    return string.Empty;
            }
        }
    }
}