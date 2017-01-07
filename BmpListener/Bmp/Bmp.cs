using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpListener.Bmp
{
    public enum MessageType : byte
    {
        RouteMonitoring,
        StatisticsReport,
        PeerDown,
        PeerUp,
        Initiation,
        Termination,
        RouteMirroring
    }

    public enum PeerType : byte
    {
        Global,
        RD,
        Local
    }
}
