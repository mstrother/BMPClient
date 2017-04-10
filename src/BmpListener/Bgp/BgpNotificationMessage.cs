using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public sealed class BgpNotificationMessage : BgpMessage
    {
        public NotificationErrorCode ErrorCode { get; private set; }
        public int ErrorSubCode { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            ErrorCode = (NotificationErrorCode)data[offset];
            ErrorSubCode = data.ElementAt(offset + 1);
        }
    }
}