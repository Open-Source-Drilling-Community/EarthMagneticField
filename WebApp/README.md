# EarthMagneticField webapp

The EarthMagneticField webapp is a Blazor Server user interface for the EarthMagneticField microservice. It provides:

- `Earth Geomagnetic Field Single Calculation` for one-off calculations from latitude, longitude, depth, date-time, and calculation method.
- `Earth Geomagnetic Field Batch Calculation` for managing persisted calculation orders.

It is packaged as a docker container called:

``norcedrillingearthmagneticfieldwebappclient``

It is available on dockerhub, under the digiwells organization, at:

https://hub.docker.com/?namespace=digiwells

The API (OpenApi schema) of the microservice is available and testable at:

https://dev.digiwells.no/EarthMagneticField/api/swagger (development server) 

https://app.digiwells.no/EarthMagneticField/api/swagger (production server)

The webapp itself is available at:

https://dev.digiwells.no/EarthMagneticField/webapp/EarthMagneticFieldSingleCalculation

https://app.digiwells.no/EarthMagneticField/webapp/EarthMagneticFieldSingleCalculation

The batch calculation page is available at:

https://dev.digiwells.no/EarthMagneticField/webapp/EarthMagneticField

https://app.digiwells.no/EarthMagneticField/webapp/EarthMagneticField

# Configuration

The webapp reads the following host URLs from configuration:

- `EarthMagneticFieldHostURL`
- `UnitConversionHostURL`

The EarthMagneticField API base path is `EarthMagneticField/api/`. The UnitConversion API base path is `UnitConversion/api/`.

# Funding

The current work has been funded by the [Research Council of Norway](https://www.forskningsradet.no/) and [Industry partners](https://www.digiwells.no/about/board/) in the framework of the cent for research-based innovation [SFI Digiwells (2020-2028)](https://www.digiwells.no/) focused on Digitalization, Drilling Engineering and GeoSteering.

# Contributors

**Lucas Volpi**, *NORCE Energy Modelling and Automation*

**Eric Cayeux**, *NORCE Energy Modelling and Automation*
