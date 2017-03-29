using BmpListener.Bmp;
using System;
using Xunit;

namespace BmpListener.Tests
{
    public class BmpTests
    {
        [Fact]
        public void ValidateBmpMessageTypes()
        {
            Assert.Equal((int)BmpMessageType.RouteMonitoring, 0);
            Assert.Equal((int)BmpMessageType.StatisticsReport, 1);
            Assert.Equal((int)BmpMessageType.PeerDown, 2);
            Assert.Equal((int)BmpMessageType.PeerUp, 3);
            Assert.Equal((int)BmpMessageType.Initiation, 4);
            Assert.Equal((int)BmpMessageType.Termination, 5);
            Assert.Equal((int)BmpMessageType.RouteMirroring, 6);
        }

        [Theory]
        [InlineData("AwAAAAYE")]
        public void BmpHeaderDecodes(string value)
        {
            var data = Convert.FromBase64String(value);
            var bmpHeader = new BmpHeader(data);
            Assert.True(bmpHeader.MessageLength == 6);
            Assert.True(bmpHeader.Version == 3);
            Assert.True(bmpHeader.MessageType == BmpMessageType.Initiation);
        }
    }
}
