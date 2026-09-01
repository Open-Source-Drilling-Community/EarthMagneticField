# WebApp

Server-side Blazor host for `WebPages`, served below `/EarthMagneticField/webapp`. `/EarthMagneticField/webapp/EarthMagneticField` redirects to `/EarthMagneticField/webapp/Home` for OSDC discovery-page compatibility.

Magnetic-field calculations remain stateless. The Service separately persists cumulative usage counters in its `/home` volume, so its JSON snapshot deployment should remain at one writer replica.

The checked-in Development settings call `https://dev.digiwells.no/`; production defaults to `http://osdcearthmagneticfieldservice/`. To use the checked-in local Service launch profile, set `EarthMagneticFieldHostURL=http://localhost:58952/`. Run the WebApp and browse to `http://localhost:58954/EarthMagneticField/webapp/Home`.

Docker image: `digiwells/osdcdrillingearthmagneticfieldwebappclient`.

Author: Eric Cayeux

Company: NORCE Research
