﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMPClient.BGP
{
    public class BGPKeepAliveMsg : ImsgBody
    {
        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}
