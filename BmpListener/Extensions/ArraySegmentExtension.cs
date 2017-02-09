using System;
using System.Linq;

namespace BmpListener.Extensions
{
    public static class ArraySegmentExtension
    {
        private static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

        public static int ToInt16(this ArraySegment<byte> data, int index)
        {
            var bytes = IsLittleEndian
                ? data.Skip(index).Take(2).Reverse() :
                data.Skip(index).Take(2);
            return BitConverter.ToInt16(bytes.ToArray(), 0);
        }
        
        public static int ToInt32(this ArraySegment<byte> data, int index)
        {
            var bytes = IsLittleEndian
                ? data.Skip(index).Take(4).Reverse() :
                data.Skip(index).Take(4);
            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }

        public static int ToInt32(this byte[] data, int index)
        {
            var bytes = IsLittleEndian
                ? data.Skip(index).Take(4).Reverse() :
                data.Skip(index).Take(4);
            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }        
    }
}