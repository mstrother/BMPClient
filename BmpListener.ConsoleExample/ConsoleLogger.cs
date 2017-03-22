using BmpListener.Bgp;
using BmpListener.Bmp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BmpListener.ConsoleExample
{
    public class ConsoleLogger
    {
        DateTime serverStartTime = DateTime.Now;

        int msgTotalCounter;
        int initiationMsgCounter;
        int peerDownMsgCounter;
        int peerUpMsgCounter;
        int routeMonitoringMsgCounter;
        int statisticsReportMsgCounter;
        int terminationMsgCounter;
        int bgpUpdateCounter;
        int prefixUpdateCounter;
        int prefixWithdrawCounter;

        HashSet<int> asns;

        public ConsoleLogger()
        {
            asns = new HashSet<int>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            while (!cancellationToken.IsCancellationRequested)
            {
                var runtime = DateTime.Now - serverStartTime;
                var bmpMsgsPerSec = ((double)msgTotalCounter / runtime.TotalSeconds).ToString("F");
                var bgpUpdateMsgsPerSec = ((double)bgpUpdateCounter / runtime.TotalSeconds).ToString("F");

                Console.SetCursorPosition(0, 5);
                Console.WriteLine($"Runtime : {FormatRuntime(runtime)}");
                Console.WriteLine();
                Console.WriteLine($"BMP messages received: {msgTotalCounter} ({bmpMsgsPerSec} messages / second)");
                Console.WriteLine($"  {routeMonitoringMsgCounter} Route Monitoring Messages");
                Console.WriteLine($"  {statisticsReportMsgCounter} Statistics Reports");
                Console.WriteLine($"  {peerDownMsgCounter} Peer Down Notifications");
                Console.WriteLine($"  {peerUpMsgCounter} Peer Up Notifications");
                Console.WriteLine($"  {initiationMsgCounter} Initiation Message");
                Console.WriteLine($"  {terminationMsgCounter} Termination Messages");
                Console.WriteLine();
                Console.WriteLine($" BGP Update Messages: {bgpUpdateCounter} ({bgpUpdateMsgsPerSec} messages / second)");
                Console.WriteLine($"  {prefixUpdateCounter} Prefix Updates");
                Console.WriteLine($"  {prefixWithdrawCounter} Prefix Withdrawals");
                Console.WriteLine();
                Console.WriteLine($"Unique Autonomous Systems: {asns.Count}");
                await Task.Delay(1000);
            }

            Console.WriteLine();
            Console.WriteLine("Shutting down...");
        }

        public async void LogMessageAsync(object sender, MessageReceivedEventArgs e)
        {
            msgTotalCounter++;
            var msg = e.BmpMessage;
            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessageType.RouteMonitoring):
                    routeMonitoringMsgCounter++;
                    var bgpMsg = ((RouteMonitoring)msg).BgpMessage;
                    if (bgpMsg.Header.Type == BgpMessageType.Update)
                    {
                        bgpUpdateCounter++;
                        var bgpUpdate = (BgpUpdateMessage)bgpMsg;
                        if (bgpUpdate.Nlri?.Count > 0)
                        {
                            prefixUpdateCounter++;
                        }
                        if (bgpUpdate.WithdrawnRoutes?.Count > 0)
                        {
                            prefixWithdrawCounter++;
                        }
                        var asPath = bgpUpdate.Attributes?.FirstOrDefault(x => x.AttributeType == PathAttributeType.AS_PATH);
                        if (asPath != null)
                        {
                            var asnCount = ((PathAttributeASPath)asPath).ASPaths[0].ASNs.Count;
                            for (int i = 1; i < asnCount; i++)
                            {
                                var asn = ((PathAttributeASPath)asPath).ASPaths[0].ASNs[i];
                                asns.Add(asn);
                            }
                        }
                    };
                    break;
                case (BmpMessageType.StatisticsReport):
                    statisticsReportMsgCounter++;
                    break;
                case (BmpMessageType.PeerDown):
                    peerDownMsgCounter++;
                    break;
                case (BmpMessageType.PeerUp):
                    peerUpMsgCounter++;
                    break;
                case (BmpMessageType.Initiation):
                    initiationMsgCounter++;
                    break;
                case (BmpMessageType.Termination):
                    terminationMsgCounter++;
                    break;
            }
        }

        static string FormatRuntime(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? String.Empty : "s") : string.Empty);

            if (formatted.EndsWith(", "))
            {
                formatted = formatted.Substring(0, formatted.Length - 2);
            }

            if (string.IsNullOrEmpty(formatted))
            {
                formatted = "0 seconds";
            }

            return formatted;
        }

    }
}
