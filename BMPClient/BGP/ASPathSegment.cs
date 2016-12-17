namespace BMPClient.BGP
{
    public class ASPathSegment
    {
        public BGP.SegmentType SegmentType { get; set; }
        public byte Length { get; set; }
        public uint[] ASNs { get; set; }
        //    Type = data[0];
        //{

        //public void DecodeFromBytes(byte[] data)
        //    Num = data[1];

        //    for (int i = 0; i < (int)Num; i++)
        //    {
        //        //a.AS = append(a.AS, binary.BigEndian.Uint16(data))
        //        //data = data[2:]
        //    }
        //}
    }
}