using BmpListener.Bmp;
using System;
using System.Net;
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
            var data = new ArraySegment<byte>(Convert.FromBase64String(value));
            var bmpHeader = new BmpHeader();
            bmpHeader.Decode(data);
            Assert.True(bmpHeader.MessageLength == 6);
            Assert.True(bmpHeader.Version == 3);
            Assert.True(bmpHeader.MessageType == BmpMessageType.Initiation);
        }

        [Theory]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAKn+qf4AAPwDLT8hK1jlelsAAAAA")]
        public void BmpPerPeerHeaderDecodes(string value)
        {
            var data = new ArraySegment<byte>(Convert.FromBase64String(value));
            var bmpPeerHeader = new PerPeerHeader();
            bmpPeerHeader.Decode(data);
            Assert.Equal(bmpPeerHeader.AS, 64515);
            Assert.Equal(bmpPeerHeader.PeerAddress, IPAddress.Parse("169.254.169.254"));
            Assert.Equal(bmpPeerHeader.PeerId, IPAddress.Parse("45.63.33.43"));
            Assert.Equal(bmpPeerHeader.PeerDistinguisher, (ulong)0);
            Assert.False(bmpPeerHeader.IsPostPolicy);
            Assert.Equal(bmpPeerHeader.PeerType, PerPeerHeader.Type.Global);
        }

        [Theory]
        [InlineData("AAAAAAAAAAAAAAAAaJz9FQCz4sP/////////////////////AC0BBFugAFponP0VEAIOAgABBAABAAFBBAAGCVv/////////////////////ADcBBPwDAPAtPyErGgIYAQQAAQABAgBABgB4AAEBAEEEAAD8A0YA")]
        public void BmpPeerUpNotificationDecodes(string value)
        {
            var data = new ArraySegment<byte>(Convert.FromBase64String(value));
            var bmpMsg = new PeerUpNotification();
            bmpMsg.Decode(data);
            //Assert.True(bmpMsg.LocalAddress == IPAddress.Parse(""));
            Assert.True(bmpMsg.RemotePort == 179);
            Assert.True(bmpMsg.LocalPort == 179);
            Assert.NotNull(bmpMsg.ReceivedOpenMessage);
            Assert.NotNull(bmpMsg.SentOpenMessage);
        }
    }
}
