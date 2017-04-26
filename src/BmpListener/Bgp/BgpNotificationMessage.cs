namespace BmpListener.Bgp
{
    public class BgpNotificationMessage : BgpMessage
    {
        public NotificationErrorCode ErrorCode { get; private set; }
        public int ErrorSubCode { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            ErrorCode = (NotificationErrorCode)data[offset];
            offset++;

            ErrorSubCode = data[offset];
        }
    }
}