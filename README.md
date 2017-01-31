# BMPListener
![BuildStatus](https://9909877.visualstudio.com/_apis/public/build/definitions/59a45cee-5267-4662-9f5b-121e6552c3cf/1/badge)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

BMPListener provides a convenient library for the collection of BGP routing information via ([BGP Monitoring Protocol](https://tools.ietf.org/html/rfc7854)). A single instance of BMPListener can handle multiple BMP devices.


# Example Output
```
{"version":"0.0.1","id":"0d45ba4da1a84ab1a44b24871bff2d14","dateTime":"2017-01-25T21:40:31Z","peer":{"type":"global","isPostPolicy":false,"distinguisher":0,"ip":"2001:470:d6:70::1","as":6939,"id":"64.71.128.26"},"attributes":{"origin":"igp","asPath":[6939,8881,48778],"atomicAggregate":true,"aggregator":{"as":48778,"ip":"141.88.254.253"}},"announce":{"ipv6 unicast":{"nexthop":"2001:470:d6:70::1","routes":["2001:67c:19c0::/48"]}}}
```

# Router Configurations

BMP is currently supported on Cisco devices running IOS XE 3.12.0/15.4.2 or above, Cisco devices running IOS XR 5.2.2, Juniper devices running JunOS 13.3 or above, and [GoBGP](http://osrg.github.io/gobgp/).
