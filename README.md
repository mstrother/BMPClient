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

#### Cisco IOS XE (3.12.0/15.4.2 or above)

This example shows how to enter BMP configuration mode and configure a Cisco IOS XE device to send BMP message to a monitoring station running on IP address 10.1.1.1, port 11019.

```
Device(config)# router bgp 65000
Device(config-router)# bmp server 1
Device(config-router-bmpsrvr)# address 10.1.1.1 port-number 11019
Device(config-router-bmpsrvr)# description LINE SERVER1
Device(config-router-bmpsrvr)# failure-retry-delay 60
Device(config-router-bmpsrvr)# flapping-delay 120
Device(config-router-bmpsrvr)# initial-delay 10
Device(config-router-bmpsrvr)# set ip dscp 5
Device(config-router-bmpsrvr)# stats-reporting-period 300
Device(config-router-bmpsrvr)# update-source GigabitEthernet1
Device(config-router-bmpsrvr)# activate
Device(config-router-bmpsrvr)# exit-bmp-server-mode
Device(config-router)# bmp-activate all
```
