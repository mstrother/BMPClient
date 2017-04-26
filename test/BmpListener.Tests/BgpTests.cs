using BmpListener.Bgp;
using System;
using System.Net;
using Xunit;

namespace BmpListener.Tests
{
    public class BgpTests
    {
        [Theory]
        [InlineData(BgpMessageType.Open, 1)]
        [InlineData(BgpMessageType.Update, 2)]
        [InlineData(BgpMessageType.Notification, 3)]
        [InlineData(BgpMessageType.Keepalive, 4)]
        [InlineData(BgpMessageType.RouteRefresh, 5)]
        public void ValidateBgpMsgTypes(BgpMessageType msgType, int i)
        {
            Assert.Equal((int)msgType, i);
        }

        [Theory]
        [InlineData(CapabilityCode.Multiprotocol, 1)]
        [InlineData(CapabilityCode.RouteRefresh, 2)]
        [InlineData(CapabilityCode.GracefulRestart, 64)]
        [InlineData(CapabilityCode.FourOctetAs, 65)]
        [InlineData(CapabilityCode.AddPath, 69)]
        [InlineData(CapabilityCode.EnhancedRouteRefresh, 70)]
        [InlineData(CapabilityCode.CiscoRouteRefresh, 128)]
        public void ValidateCapabilityCodes(CapabilityCode code, int i)
        {
            Assert.Equal((int)code, i);
        }

        [Theory]
        [InlineData(PathAttributeType.Origin, 1)]
        [InlineData(PathAttributeType.AsPath, 2)]
        [InlineData(PathAttributeType.NextHop, 3)]
        [InlineData(PathAttributeType.MultiExitDisc, 4)]
        [InlineData(PathAttributeType.LocalPref, 5)]
        [InlineData(PathAttributeType.AtomicAggregate, 6)]
        [InlineData(PathAttributeType.Aggregator, 7)]
        [InlineData(PathAttributeType.Community, 8)]
        [InlineData(PathAttributeType.OriginatorId, 9)]
        [InlineData(PathAttributeType.ClusterList, 10)]
        [InlineData(PathAttributeType.MpReachNlri, 14)]
        [InlineData(PathAttributeType.ExtendedCommunities, 16)]
        [InlineData(PathAttributeType.As4Path, 17)]
        [InlineData(PathAttributeType.As4Aggregator, 18)]
        [InlineData(PathAttributeType.PmsiTunnel, 22)]
        [InlineData(PathAttributeType.TunnelEncap, 23)]
        [InlineData(PathAttributeType.Aigp, 26)]
        [InlineData(PathAttributeType.LargeCommunity, 32)]
        public void ValidatePathAttributeTypes(PathAttributeType attr, int i)
        {
            Assert.Equal((int)attr, i);
        }

        [Theory]
        [InlineData("/////////////////////wAtAQ==")]
        public void BgpHeaderDecodes(string value)
        {
            var data = Convert.FromBase64String(value);
            var bgpHeader = new BgpHeader();
            bgpHeader.Decode(data, 0);
            Assert.Equal(bgpHeader.Length, 45);
            Assert.Equal(bgpHeader.Type, BgpMessageType.Open);
        }

        [Theory]
        [InlineData("BFugAFponP0VEAIOAgABBAABAAFBBAAGCVv/////////////////////ADcBBPwDAPAtPyErGgIYAQQAAQABAgBABgB4AAEBAEEEAAD8A0YA")]
        public void BgpOpenDecodes(string value)
        {
            var data = Convert.FromBase64String(value);
            var msg = new BgpOpenMessage();
            msg.Decode(data, 0);
            Assert.Equal(msg.MyAS, 23456);
            Assert.Equal(msg.Version, 4);
            Assert.Equal(msg.HoldTime, 90);
        }

        [Theory]
        [InlineData("FVtiYA==")]
        public void IPv4PrefixDecodes(string value)
        {
            var data = Convert.FromBase64String(value);
            (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, 0, AddressFamily.IP);
            Assert.Equal(prefix.Prefix, IPAddress.Parse("91.98.96.0"));
            Assert.Equal(prefix.Length, 21);
            Assert.Equal(byteLength, 4);
            Assert.Equal(prefix.ToString(), "91.98.96.0/21");
        }

        [Theory]
        [InlineData("LCYGroAUEA ==")]
        public void IPv6PrefixDecodes(string value)
        {
            var data = Convert.FromBase64String(value);
            (IPAddrPrefix prefix, int byteLength) = IPAddrPrefix.Decode(data, 0, AddressFamily.IP6);
            Assert.Equal(prefix.Prefix, IPAddress.Parse("2606:ae80:1410::"));
            Assert.Equal(prefix.Length, 44);
            Assert.Equal(byteLength, 7);
            Assert.Equal(prefix.ToString(), "2606:ae80:1410::/44");
        }

        [Theory]
        [InlineData("AAYJWw==")]
        public void CapabilityFourOctetASDecodes(string value)
        {
            var data = Convert.FromBase64String(value);
            var capability = new CapabilityFourOctetAsNumber();
            capability.Decode(data, 0);
            Assert.Equal(395611, capability.Asn);
        }

        [Theory]
        [InlineData("AAEAAQ==")]
        public void CapabilityMultiProtocolDecodes(string value)
        {
            var data = Convert.FromBase64String(value);
            var capability = new CapabilityMultiProtocol();
            capability.Decode(data, 0);
            Assert.Equal(capability.Afi, AddressFamily.IP);
            Assert.Equal(capability.Safi, SubsequentAddressFamily.Unicast);
        }
    }
}
