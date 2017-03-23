using BmpListener.Bmp;
using System;
using Xunit;

namespace BmpListener.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void BmpHeaderDecodes()
        {
            var data = new byte[] { 3, 0, 0, 0, 6, 3 };
            var bmpHeader = new BmpHeader(data);        
            Assert.True(bmpHeader.Version == 3);
            Assert.True(bmpHeader.MessageLength == 6);
            Assert.True(bmpHeader.MessageType == BmpMessageType.Initiation);
        }
    }
}
