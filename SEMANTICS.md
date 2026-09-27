# Semantic bindings

The Model uses published `OSDC.DotnetLibraries.Drilling.SemanticCatalogue` 0.4.0 from NuGet, without a sibling project reference. That catalogue contains the curated EarthGravity, digest and EarthMagneticField vocabulary. Provider-owned `Semantic` attributes are authoritative for the bindings in this service.

`SemanticSchemaFilter` exports them in REST/OpenAPI as `x-osdc-semantic`. MCP applies the same attributes to request, response, inline evaluation-point and named result/provenance schemas. Model information is shared between MCP discovery and evaluation. Generated merged OpenAPI retains the annotations; the C# client keeps its existing JSON property names and value types.

| Data | Concept and role/reference |
| --- | --- |
| Evaluation point | Geodetic evaluation point, containing position and instant |
| Latitude / Longitude / Depth | Geodetic latitude / longitude / ellipsoidal depth, WGS84 |
| DateTimeUtc | Instant, UTC; no duration quantity |
| North / East / Down | Earth magnetic flux density with the respective component role, NED |
| HorizontalIntensity / TotalIntensity | Earth magnetic flux density with horizontal-magnitude / vector-magnitude role |
| MagneticDip / Declination | Magnetic dip / magnetic declination, NED; PlaneAngleDrilling |
| Model information | Geomagnetic model provenance |
| Model selection | Provider model-selection token, distinct from model name and ID |
| MinimumUtc / MaximumUtc | Instant with lower/upper bound role, UTC |
| MinimumDepth / MaximumDepth | Ellipsoidal depth with lower/upper bound role, WGS84 |
| MetadataSHA256 / CoefficientSHA256 | SHA-256 file digest with model-metadata-file / coefficient-file role |

Magnetic values are flux density B in teslas, not field strength in amperes per metre. Magnetic dip is positive downward from the local horizontal and null when the total vector is zero. Declination is positive east of geodetic north and null when the horizontal projection is zero. Validity bounds are inclusive. Digest values encode hashes of complete file bytes as 64 lowercase hexadecimal characters. Model ReleaseDate is a calendar date, not the evaluation instant.

Provider schemas continue to own validation, nullability, allowed enum values and execution policies. REST defaults an omitted Model to WMM2025; MCP requires an explicit Model. Semantic annotations neither change these rules nor enforce all required context automatically. Service identity, concurrency declarations and unit/frame strings remain provider metadata rather than invented scientific nouns.

## Verification and release

Service tests compare every annotated engineering model/property against REST and MCP metadata, including quantity resolution, catalogue version/status, UTC bounds and digest roles. Existing numerical, generated-client, validation and MCP execution tests remain applicable.

Refresh the service OpenAPI and run ModelSharedOut after model changes, following the generation instructions in the root README. Both Dockerfiles already support the package-only dependency layout; no DotNetLibraries checkout is needed. This source upgrade does not publish packages, build/push Docker images or redeploy running services.
