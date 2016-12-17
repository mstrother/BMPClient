# BMPClient

A simple BMP ([BGP Monitoring Protocol](https://tools.ietf.org/html/rfc7854)) client. Currently it just shows all received BMP messages on the console in JSON. 

```
{"Header":{"Version":3,"Length":123,"Type":0},"PeerHeader":{"PeerType":0,"IsPostPolicy":false,"PeerDistinguisher":0,"PeerAddress":"2001:470:d6:70::1","PeerAS":6939,"PeerBGPId":"64.71.128.26","Timestamp":"2016-12-16T13:28:31Z","Flags":128},"Body":{"BGPUpdate":{"Header":{"Length":75,"Type":2},"Body":{"WithdrawnRoutesLength":0,"WithdrawnRoutes":null,"TotalPathAttributeLength":52,"PathAttributes":[{"Origin":0,"Flags":64,"Type":1,"Length":1},{"ASPaths":[{"SegmentType":2,"Length":3,"ASNs":[6939,14840,264555]}],"Flags":64,"Type":2,"Length":14},{"NextHop":"2001:470:d6:70::1","LinkLocalNextHop":null,"AFI":2,"SAFI":1,"Value":["2804:2174:cb::/48"],"Flags":128,"Type":14,"Length":28}]}}}}
```
