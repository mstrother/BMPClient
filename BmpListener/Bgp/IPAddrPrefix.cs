﻿using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using BmpListener.Serialization;
using BmpListener.Serialization.JsonConverters;

namespace BmpListener.Bgp
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class IPAddrPrefix
    {
        public IPAddrPrefix(ArraySegment<byte> data, Bgp.AddressFamily afi = Bgp.AddressFamily.IPv4)
        {
            DecodeFromBytes(data, afi);
        }

        public byte Length { get; private set; }
        public IPAddress Prefix { get; private set; }

        public override string ToString()
        {
            return ($"{Prefix}/{Length}");
        }

        public void DecodeFromBytes(ArraySegment<byte> data, Bgp.AddressFamily afi)
        {
            //add length error check
            Length = data.ElementAt(0);
            var byteLength = (Length + 7) / 8;
            var ipBytes = afi == Bgp.AddressFamily.IPv4
                ? new byte[4]
                : new byte[16];
            if (Length <= 0) return;
            Buffer.BlockCopy(data.ToArray(), 1, ipBytes, 0, byteLength);
            Prefix = new IPAddress(ipBytes);
        }
    }
}
