using System;

namespace BmpListener
{
    public static class BigEndian
    {
        public static short ToInt16(ArraySegment<byte> data, int offset)
        {
            offset += data.Offset;
            return (short)((data.Array[offset++] << 8) | (data.Array[offset++]));
        }

        public static ushort ToUInt16(ArraySegment<byte> data, int offset)
        {
            offset += data.Offset;
            return (ushort)((data.Array[offset++] << 8) | data.Array[offset++]);
        }

        public static int ToInt32(ArraySegment<byte> data, int offset)
        {
            offset += data.Offset;
            return (data.Array[offset++] << 24) | (data.Array[offset++] << 16)
          | (data.Array[offset++] << 8) | data.Array[offset++];
        }

        public static uint ToUInt32(ArraySegment<byte> data, int offset)
        {
            offset += data.Offset;
            return (uint)((data.Array[offset++] << 24) | (data.Array[offset++] << 16)
          | (data.Array[offset++] << 8) | data.Array[offset++]);
        }

        public static long ToInt64(ArraySegment<byte> data, int offset)
        {
            offset += data.Offset;
            return (data.Array[offset++] << 56) | (data.Array[offset++] << 48)
               | (data.Array[offset++] << 40) | (data.Array[offset++] << 32)
               | (data.Array[offset++] << 24) | (data.Array[offset++] << 16)
          | (data.Array[offset++] << 8) | data.Array[offset++];
        }

        public static ulong ToUInt64(ArraySegment<byte> data, int offset)
        {
            offset += data.Offset;
            return (ulong)((data.Array[offset++] << 56) | (data.Array[offset++] << 48)
               | (data.Array[offset++] << 40) | (data.Array[offset++] << 32)
               | (data.Array[offset++] << 24) | (data.Array[offset++] << 16)
          | (data.Array[offset++] << 8) | data.Array[offset++]);
        }
    }
}
