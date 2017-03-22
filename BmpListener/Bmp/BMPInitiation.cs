namespace BmpListener.Bmp
{
    public class BmpInitiation : BmpMessage
    {
        public BmpInitiation(byte[] data)
            : base(data)
        {
        }
    }
}