﻿using System;

namespace BmpListener.BGP
{
    public class CapabilityCiscoRouteRefresh : Capability
    {
        public CapabilityCiscoRouteRefresh(CapabilityCode capability, ArraySegment<byte> data)
            : base(capability, data)
        {
        }
    }
}