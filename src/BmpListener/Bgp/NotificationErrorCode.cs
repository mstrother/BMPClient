namespace BmpListener.Bgp
{
    // RFC 4271 4.5
    public enum NotificationErrorCode
    {
        MessageHeaderError = 1,
        OpenMessageError,
        UpdateMessageError,
        HoldTimerExpired,
        FiniteStateMachineError,
        Cease,
        RouteRefreshMessageError
    }
}
