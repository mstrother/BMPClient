# BMPListener
![BuildStatus](https://9909877.visualstudio.com/_apis/public/build/definitions/59a45cee-5267-4662-9f5b-121e6552c3cf/1/badge)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

BMPListener provides a convenient library for the collection of BGP routing information via ([BGP Monitoring Protocol](https://tools.ietf.org/html/rfc7854)). A single instance of BMPListener can handle multiple BMP devices.


# Example Output
```
{"Header":{"Version":3,"Length":123,"Type":0},"PeerHeader":{"PeerType":0,"IsPostPolicy":false,"PeerDistinguisher":0,"PeerAddress":"2001:470:d6:70::1","PeerAS":6939,"PeerBGPId":"64.71.128.26","Timestamp":"2016-12-16T13:28:31Z","Flags":128},"Body":{"BGPUpdate":{"Header":{"Length":75,"Type":2},"Body":{"WithdrawnRoutesLength":0,"WithdrawnRoutes":null,"TotalPathAttributeLength":52,"PathAttributes":[{"Origin":0,"Flags":64,"Type":1,"Length":1},{"ASPaths":[{"SegmentType":2,"Length":3,"ASNs":[6939,14840,264555]}],"Flags":64,"Type":2,"Length":14},{"NextHop":"2001:470:d6:70::1","LinkLocalNextHop":null,"AFI":2,"SAFI":1,"Value":["2804:2174:cb::/48"],"Flags":128,"Type":14,"Length":28}]}}}}
```

# Router Configurations

BMP is currently supported on Cisco devices running IOS XE 3.12.0/15.4.2 or above, Cisco devices running IOS XR 5.2.2, Juniper devices running JunOS 13.3 or above, and [GoBGP](http://osrg.github.io/gobgp/).
