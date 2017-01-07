using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpListener.Bgp
{
    public interface IBgpMessage
    {
        void DecodeFromBytes(ArraySegment<byte> data);
    }
}
