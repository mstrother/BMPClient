using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmpListener.Bmp
{
    public enum MessageType
    {
        RouteMonitoring,
        StatisticsReport,
        PeerDown,
        PeerUp,
        Initiation,
        Termination,
        RouteMirroring
    }

    public enum PeerType
    {
        Global,
        RD,
        Local
    }

    public enum PeerDownReason
    {
        Unknown = 0,
        LocalNotification,
        LocalNoNotification,
        RemoteNotification,
        LocationNoNotification
    }
}
