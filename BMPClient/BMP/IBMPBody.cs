namespace BmpListener.Bmp
{
    public interface IBMPBody
    {
        void ParseBody(BmpMessage message, byte[] messageBytes);
    }
}