using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public sealed class BgpNotificationMessage : BgpMessage
    {
        public NotificationErrorCode ErrorCode { get; private set; }
        public int ErrorSubCode { get; private set; }

        public override void Decode(ArraySegment<byte> data)
        {
            ErrorCode = (NotificationErrorCode)data.First();
            ErrorSubCode = data.ElementAt(1);
        }
    }
}