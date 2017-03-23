﻿using BmpListener.Bgp;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BmpListener.Tests
{
    public class BgpTests
    {
        [Fact]
        public void BmpHeaderDecodes()
        {
            var data = new byte[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 45, 1 };
            var bgpHeader = new BgpHeader(data, 3);
            Assert.True(bgpHeader.Length == 45);
            Assert.True(bgpHeader.Type == BgpMessageType.Open);
        }

        [Fact]
        public void BmpMessageTypes()
        {
            Assert.True((int)BgpMessageType.Open == 1);
            Assert.True((int)BgpMessageType.Update == 2);
            Assert.True((int)BgpMessageType.Notification == 3);
            Assert.True((int)BgpMessageType.Keepalive == 4);
            Assert.True((int)BgpMessageType.RouteRefresh == 5);
        }
    }
}