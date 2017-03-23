using BmpListener.Bmp;
using System;
using Xunit;

namespace BmpListener.Tests
{
    public class BmpTests
    {
        [Fact]
        public void BmpHeaderDecodes()
        {
            var data = new byte[] { 3, 0, 0, 0, 6, 4 };
            var bmpHeader = new BmpHeader(data);
            Assert.True(bmpHeader.Version == 3);
            Assert.True(bmpHeader.MessageLength == 6);
            Assert.True(bmpHeader.MessageType == BmpMessageType.Initiation);
        }

        [Fact]
        public void BmpMessageTypes()
        {
            Assert.True((int)BmpMessageType.RouteMonitoring == 0);
            Assert.True((int)BmpMessageType.StatisticsReport == 1);
            Assert.True((int)BmpMessageType.PeerDown == 2);
            Assert.True((int)BmpMessageType.PeerUp == 3);
            Assert.True((int)BmpMessageType.Initiation == 4);
            Assert.True((int)BmpMessageType.Termination == 5);
            Assert.True((int)BmpMessageType.RouteMirroring == 6);
        }
    }
}
