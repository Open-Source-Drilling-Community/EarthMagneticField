# EarthMagneticField

The EarthMagneticField repository hosts the EarthMagneticField microservice, reusable Razor pages, and a client webapp for single and batch earth geomagnetic field calculations.

# Solution architecture

The solution is composed of:
- **ModelSharedIn**
  - contains C# auto-generated classes of Model dependencies
  - these dependencies are stored as json files (following the OpenApi standard) and C# classes are generated on execution of the program
  - *dependencies* = some external microservices (OpenApi schemas in json format)
- **Model**
  - defines the main classes and methods to run the microservice
  - *dependencies* = BaseModels
- **Service**
  - defines the proper microservice API
  - *dependencies* = Model
- **ModelSharedOut**
  - contains C# auto-generated classes for microservice clients dependencies
  - these dependencies are stored as json files (following the OpenAPI standard) and C# classes are generated on execution of the program
  - these dependencies include the OpenApi schema of the microservice itself as well as other dependencies that may be useful to run the microservice
  - *dependencies* = EarthMagneticField.json + some external microservices (OpenApi schemas in json format)
- **ModelTest**
  - performs unit tests on the Model (in particular for base computations)
  - *dependencies* = Model
- **ServiceTest**
  - microservice client that performs unit tests on the microservice (by default, an instance of the microservice must be running on http port 8080 to run tests)
  - *dependencies* = ModelShared
- **WebApp**
  - Blazor Server webapp that hosts single and batch earth geomagnetic field calculation pages
  - *dependencies* = WebPages
- **WebPages**
  - reusable Razor class library containing the EarthMagneticField web pages
  - includes home, single calculation, batch calculation list/edit, and result view pages
  - *dependencies* = ModelSharedOut + WebAppUtils + DrillingRazorMudComponents
- **home** (auto-generated)
  - data are persisted in the microservice container using the Sqlite database located at *home/EarthMagneticField.db*

# Security/Confidentiality

Data are persisted as clear text in a unique Sqlite database hosted in the docker container.
Neither authentication nor authorization have been implemented.
Would you like or need to protect your data, docker containers of the microservice and webapp are available on dockerhub, under the digiwells organization, at:

https://hub.docker.com/?namespace=digiwells

More info on how to run the container and map its database to a folder on your computer, at:

https://github.com/NORCE-DrillingAndWells/DrillingAndWells/wiki

# Deployment

Microservice is available at:

https://dev.digiwells.no/EarthMagneticField/api/EarthMagneticFieldCalculationOrder

https://app.digiwells.no/EarthMagneticField/api/EarthMagneticFieldCalculationOrder

Web app is available at:

https://dev.digiwells.no/EarthMagneticField/webapp/EarthMagneticFieldSingleCalculation

https://app.digiwells.no/EarthMagneticField/webapp/EarthMagneticFieldSingleCalculation

Batch calculation management is available at:

https://dev.digiwells.no/EarthMagneticField/webapp/EarthMagneticField

https://app.digiwells.no/EarthMagneticField/webapp/EarthMagneticField

The OpenApi schema of the microservice is available and testable at:

https://dev.digiwells.no/EarthMagneticField/api/swagger (development server) 

https://app.digiwells.no/EarthMagneticField/api/swagger (production server)

The microservice and webapp are deployed as Docker containers using Kubernetes and Helm. More info at:

https://github.com/NORCE-DrillingAndWells/DrillingAndWells/wiki

# Funding

The current work has been funded by the [Research Council of Norway](https://www.forskningsradet.no/) and [Industry partners](https://www.digiwells.no/about/board/) in the framework of the cent for research-based innovation [SFI Digiwells (2020-2028)](https://www.digiwells.no/) focused on Digitalization, Drilling Engineering and GeoSteering. 

# Contributors

**Lucas Volpi**, *NORCE Energy Modelling and Automation*

**Eric Cayeux**, *NORCE Energy Modelling and Automation*
