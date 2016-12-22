namespace BmpListener.BMP
{
    public interface IBMPBody
    {
        void ParseBody(BMPMessage message, byte[] messageBytes);
    }
}