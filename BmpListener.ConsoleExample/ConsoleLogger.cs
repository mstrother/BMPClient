using BmpListener.Bmp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BmpListener.ConsoleExample
{
    public class ConsoleLogger
    {
        DateTime serverStartTime = DateTime.Now;

        int msgTotalCounter;
        int initiationMsgTotalCounter;
        int peerDownMsgTotalCounter;
        int peerUpMsgTotalCounter;
        int routeMonitoringMsgTotalCounter;
        int statisticsReportMsgTotalCounter;
        int terminationMsgTotalCounter;
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            while (!cancellationToken.IsCancellationRequested)
            {
                var runtime = DateTime.Now - serverStartTime;
                var msgsPerSec = (msgTotalCounter / (decimal)runtime.TotalSeconds).ToString("0.##");

                Console.SetCursorPosition(0, 5);
                Console.WriteLine($"Runtime : {FormatRuntime(runtime)}");
                Console.WriteLine();
                Console.WriteLine($"{msgTotalCounter} BMP messages received:");
                Console.WriteLine($"  {routeMonitoringMsgTotalCounter} Route Monitoring Messages");
                Console.WriteLine($"  {statisticsReportMsgTotalCounter} Statistics Reports");
                Console.WriteLine($"  {peerDownMsgTotalCounter} Peer Down Notifications");
                Console.WriteLine($"  {peerUpMsgTotalCounter} Peer Up Notifications");
                Console.WriteLine($"  {initiationMsgTotalCounter} Initiation Message");
                Console.WriteLine($"  {terminationMsgTotalCounter} Termination Messages");
                Console.WriteLine();
                Console.WriteLine("BMP messages/second : {0}", msgsPerSec);
                await Task.Delay(1000);
            }

            Console.WriteLine();
            Console.WriteLine("Shutting down...");
        }

        public void LogMessage(object sender, MessageReceivedEventArgs e)
        {
            msgTotalCounter++;
            var msg = e.BmpMessage;
            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessageType.RouteMonitoring):
                    routeMonitoringMsgTotalCounter++;
                    break;
                case (BmpMessageType.StatisticsReport):
                    statisticsReportMsgTotalCounter++;
                    break;
                case (BmpMessageType.PeerDown):
                    peerDownMsgTotalCounter++;
                    break;
                case (BmpMessageType.PeerUp):
                    peerUpMsgTotalCounter++;
                    break;
                case (BmpMessageType.Initiation):
                    initiationMsgTotalCounter++;
                    break;
                case (BmpMessageType.Termination):
                    terminationMsgTotalCounter++;
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
