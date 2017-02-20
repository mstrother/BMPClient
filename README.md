# BMPListener
![BuildStatus](https://9909877.visualstudio.com/_apis/public/build/definitions/59a45cee-5267-4662-9f5b-121e6552c3cf/1/badge)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

BMPListener provides a convenient library for the collection and serialization of BGP routing information via ([BGP Monitoring Protocol](https://tools.ietf.org/html/rfc7854)). A single instance of BMPListener can handle multiple BMP devices.

BMPListener includes a JSON serializer.


# Example Output
```
{"version":"0.1.0","id":"0a19386080b44429b146ab121cc58843","dateTime":"2017-02-17T07:43:47+00:00","peer":{"asn":64515,"ip":"169.254.169.254","id":"45.63.33.43","distinguisher":0,"type":"global","postPolicy":false},"update":{"origin":"igp","asPath":[64515,20473,2914,3491,4760],"atomicAggregate":false,"announce":{"nexthop":"127.0.0.1","routes":{"iPv4 Unicast":["220.246.224.0/19"]}}}}
```

# Router Configurations

BMP is currently supported on Cisco devices running IOS XE 3.12.0/15.4.2 or above, Cisco devices running IOS XR 5.2.2, Juniper devices running JunOS 13.3 or above, and [GoBGP](http://osrg.github.io/gobgp/).
