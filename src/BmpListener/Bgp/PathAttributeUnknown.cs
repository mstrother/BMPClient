using System;

namespace BmpListener.Bgp
{
    public class PathAttributeUnknown : PathAttribute
    {
        public PathAttributeUnknown(byte[] data, int offset) 
            : base(data, offset)
        {
        }        
    }
}