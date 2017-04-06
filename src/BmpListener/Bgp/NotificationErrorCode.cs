namespace BmpListener.Bgp
{
    // RFC 4271 4.5
    public enum NotificationErrorCode
    {
        MessageHeaderError = 1,
        OPENMessageError,
        UPDATEMessageError,
        HoldTimerExpired,
        FiniteStateMachineError,
        Cease,
        RouteRefreshMessageError
    }
}
