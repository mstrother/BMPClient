namespace BmpListener.BMP
{
    public class Header
    {
        public Header(byte[] data)
        {
            ParseBytes(data);
        }

        public byte Version { get; private set; }
        public uint Length { get; private set; }
        public BMPMessage.BMPMessageType Type { get; private set; }

        public void ParseBytes(byte[] data)
        {
            Version = data[0];
            //if (Version != 3)
            //{
            //    throw new Exception("invalid BMP version");
            //}
            Length = data.ToUInt32(1);
            Type = (BMPMessage.BMPMessageType) data[5];
        }
    }
}