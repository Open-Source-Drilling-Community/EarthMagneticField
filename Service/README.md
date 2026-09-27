# Service

ASP.NET Core host for the Earth Magnetic Field REST and MCP APIs. WMM2025 and IGRF14 calculations are stateless; request samples and results are never persisted. The service restores and periodically snapshots cumulative usage counters.

- `GET /EarthMagneticField/api/EarthMagneticField`: discovery and installed-model information.
- `POST /EarthMagneticField/api/EarthMagneticField/Evaluate`: atomic synchronous WMM2025/IGRF14 batch evaluation.
- `GET /EarthMagneticField/api/EarthMagneticField/ModelInfo`: full model provenance.
- `GET /EarthMagneticField/api/EarthMagneticFieldUsageStatistics`: cumulative counters persisted by the service.
- `/EarthMagneticField/api/mcp`, `/health/*`, `/metrics`, `/swagger`, and `/swagger/merged/swagger.json`.

Configuration section `EarthMagneticField` supports `MaximumSamplesPerRequest` (default 10000), optional `ModelDirectory`, `UsageStatisticsFile`, and `UsageStatisticsSaveIntervalSeconds` (default 30). In containers, snapshots default to `/home/EarthMagneticField.UsageStatistics.json`; Docker declares `/home` as a volume and Helm creates a PVC there by default. MCP exposes only `ping`, `earth_magnetic_field_get_model_info`, and `earth_magnetic_field_evaluate`; usage statistics are excluded.

Author: Eric Cayeux

Company: NORCE Research

The merged OpenAPI document advertises the API root (`/EarthMagneticField/api`) as its server URL, excluding the schema route. Service tests verify that its advertised URL and operation path resolve to a working endpoint.

Magnetic dip uses the `MagneticDip` result property and the label “Magnetic dip”. It is in SI radians, positive downward from horizontal, and nullable when total magnetic flux density is zero. Service and client releases must agree on this property name.

SemanticSchemaFilter publishes model attributes as x-osdc-semantic in OpenAPI. MCP schemas use the same attributes; discovery and evaluation share one annotated model-information schema. Required fields, nullability and execution policies remain provider-owned. REST defaults an omitted Model to WMM2025; MCP requires explicit Model selection.
