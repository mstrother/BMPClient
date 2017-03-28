using BmpListener.Bgp;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BmpListener.Tests
{
    public class BgpTests
    {
        [Fact]
        public void ValidateBgpMessageTypes()
        {
            Assert.Equal((int)BgpMessageType.Open, 1);
            Assert.Equal((int)BgpMessageType.Update, 2);
            Assert.Equal((int)BgpMessageType.Notification, 3);
            Assert.Equal((int)BgpMessageType.Keepalive, 4);
            Assert.Equal((int)BgpMessageType.RouteRefresh, 5);
        }

        [Fact]
        public void ValidateCapabilityCodes()
        {
            Assert.Equal((int)CapabilityCode.Multiprotocol, 1);
            Assert.Equal((int)CapabilityCode.RouteRefresh, 2);
            Assert.Equal((int)CapabilityCode.GracefulRestart, 64);
            Assert.Equal((int)CapabilityCode.FourOctetAs, 65);
            Assert.Equal((int)CapabilityCode.AddPath,69);
            Assert.Equal((int)CapabilityCode.EnhancedRouteRefresh, 70);
            Assert.Equal((int)CapabilityCode.CiscoRouteRefresh, 128);
        }        
    }    
}
