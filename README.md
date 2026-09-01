# OSDC Earth Magnetic Field

OSDC Earth Magnetic Field is a .NET 8 microservice with stateless WMM2025 and IGRF14 calculations for batches of WGS84 positions and UTC instants. It provides REST and MCP interfaces, a generated shared client, reusable unit-aware Blazor pages, a WebApp, Docker images, and Helm charts. There is no database, calculation-order resource, GUID workflow, or persisted calculation result; cumulative usage counters are persisted separately.

## Solution structure

- `Model`: typed contracts, validation, model provenance, usage counters, and the GeographicLib evaluator.
- `Service`: REST, MCP, health, metrics, usage statistics, OpenAPI, and JSON Schema endpoints.
- `ModelSharedOut`: generated C# client and committed OpenAPI/JSON Schema artifacts.
- `WebPages`: reusable Razor class library with OSDC unit-system controls.
- `WebApp`: server-side Blazor host.
- `ModelTest` and `ServiceTest`: numerical, validation, concurrency, REST, MCP, and operational tests.
- `MagneticFieldModelFiles`: WMM2025 and IGRF14 metadata and coefficients.

## Public conventions

- `Latitude`: WGS84 geodetic latitude in SI radians, `[-π/2, π/2]`.
- `Longitude`: WGS84 longitude in SI radians, `[-π, π]`.
- `Depth`: SI metres, positive downward from the WGS84 reference ellipsoid.
- `DateTimeUtc`: explicit UTC timestamp containing `Z` or `+00:00`.
- `North`, `East`, and `Down`: magnetic flux density in SI teslas, positive geodetic north, east, and down.
- `Declination`: SI radians, positive east of geodetic north.
- `Inclination`: SI radians, positive downward from horizontal.

GeographicLib expects degrees, positive-up ellipsoidal height, fractional year, and returns east-north-up nanoteslas. Conversion, reordering, and the vertical sign change occur only at the library boundary.

## Models

- WMM2025: UTC 2025-01-01 through 2030-01-01 inclusive; depth −850000 m through 1000 m.
- IGRF14: UTC 1900-01-01 through 2030-01-01 inclusive; depth −600000 m through 1000 m.

The model-information API exposes degree/order, exact validity bounds, release date, GeographicLib version, and SHA-256 hashes of both metadata and coefficient files.

## Run locally

```powershell
dotnet restore EarthMagneticField.sln
dotnet run --project Service/Service.csproj
```

Local endpoints:

- discovery/model information: `http://localhost:58952/EarthMagneticField/api/EarthMagneticField`
- evaluation: `POST http://localhost:58952/EarthMagneticField/api/EarthMagneticField/Evaluate`
- usage statistics: `http://localhost:58952/EarthMagneticField/api/EarthMagneticFieldUsageStatistics`
- MCP Streamable HTTP: `http://localhost:58952/EarthMagneticField/api/mcp`
- health: `http://localhost:58952/EarthMagneticField/api/health/live`
- readiness: `http://localhost:58952/EarthMagneticField/api/health/ready`
- metrics: `http://localhost:58952/EarthMagneticField/api/metrics`
- Swagger: `http://localhost:58952/EarthMagneticField/api/swagger`

Example:

```powershell
$body = '{"Model":"WMM2025","Samples":[{"Latitude":1.0471975511965976,"Longitude":0.17453292519943295,"Depth":1000.0,"DateTimeUtc":"2026-08-24T10:00:00Z"}]}'
Invoke-RestMethod -Method Post -ContentType application/json -Body $body `
  -Uri http://localhost:58952/EarthMagneticField/api/EarthMagneticField/Evaluate
```

For a WebApp connected to that local service, set `EarthMagneticFieldHostURL=http://localhost:58952/`, run `dotnet run --project WebApp/WebApp.csproj`, then browse to `http://localhost:58954/EarthMagneticField/webapp/Home`. The checked-in Development settings use `https://dev.digiwells.no/` instead.

## MCP tools

The server publishes exactly three underscore-named tools:

- `ping`
- `earth_magnetic_field_get_model_info`
- `earth_magnetic_field_evaluate`

`tools/list` includes complete descriptions and strict input/output JSON Schemas. Usage statistics remain available through REST and metrics but are intentionally excluded from MCP.

Usage-counter snapshots are stored atomically in `/home/EarthMagneticField.UsageStatistics.json`, restored during startup, written every 30 seconds when changed, and flushed during graceful shutdown. They survive pod replacement when the `/home` persistent volume is retained; an abrupt process or node failure can lose changes since the last snapshot. Override the defaults with `EarthMagneticField__UsageStatisticsFile` and `EarthMagneticField__UsageStatisticsSaveIntervalSeconds`. The JSON snapshot is a single-writer design, so keep the service at one replica.

## Generation and validation

```powershell
dotnet tool restore
dotnet restore EarthMagneticField.sln
dotnet build Service/Service.csproj -c Release --no-restore
dotnet swagger tofile --output ModelSharedOut/json-schemas/EarthMagneticFieldFullName.json Service/bin/Release/net8.0/Service.dll v1
dotnet run --project ModelSharedOut/ModelSharedOut.csproj -c Release
dotnet build EarthMagneticField.sln -c Release --no-restore
dotnet test EarthMagneticField.sln -c Release --no-build
```

Generated contracts are committed and checked for deterministic regeneration in CI.

## Docker, Kubernetes, and publishing

Docker Hub images:

- `digiwells/osdcdrillingearthmagneticfieldservice`
- `digiwells/osdcdrillingearthmagneticfieldwebappclient`

The Docker GitHub Action uses private repository secrets `DOCKERHUB_USERNAME` and `DOCKERHUB_TOKEN`. The WebPages publishing action uses `NUGET_API_KEY` and publishes `OSDC.Drilling.EarthMagneticField.WebPages`.

```powershell
docker build -f Service/Dockerfile -t digiwells/osdcdrillingearthmagneticfieldservice:local .
docker build -f WebApp/Dockerfile -t digiwells/osdcdrillingearthmagneticfieldwebappclient:local .
docker run --rm -p 8080:8080 -v earthmagneticfield-home:/home digiwells/osdcdrillingearthmagneticfieldservice:local
helm upgrade --install osdcearthmagneticfieldservice Service/charts/osdcdrillingearthmagneticfieldservice
helm upgrade --install osdcearthmagneticfieldwebapp WebApp/charts/osdcdrillingearthmagneticfieldwebappclient
```

The Service chart creates a PVC mounted at `/home` by default for statistics persistence; set `persistence.existingClaim` to reuse a managed volume. The charts create no PodDisruptionBudget. See [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md) for model and GeographicLib attribution.

Author: Eric Cayeux

Company: NORCE Research
