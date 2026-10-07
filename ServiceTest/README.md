# ServiceTest

Hosts the actual ASP.NET Core service in memory and verifies both models through the generated client, structured HTTP 422 validation, discovery routes, operational endpoints, exact MCP registration and schemas, end-to-end MCP evaluation, exclusion of usage statistics from MCP, and usage-counter restoration across a service restart.

Run `dotnet test ServiceTest/ServiceTest.csproj`.

Author: Eric Cayeux

Company: NORCE Research

The merged OpenAPI document advertises the API root (`/EarthMagneticField/api`) as its server URL, excluding the schema route. Service tests verify that its advertised URL and operation path resolve to a working endpoint.

Magnetic-dip checks verify the `MagneticDip` calculation and contract name; service tests cover REST, MCP, OpenAPI and the generated client, including removal of the former result property.

REST/MCP semantic parity tests compare every annotated model type/property against its provider attribute, including catalogue version/status, resolved quantities, UTC bounds and the two SHA-256 source-file roles. Discovery and evaluation model schemas must match.

All registered stateless MCP tools now explicitly publish all four behavior hints: read-only, idempotent, non-destructive and closed-world. The tools/list regression checks every hint. After deployment, rediscover these contracts in consuming DrillWeaver installations; redeployment alone does not refresh their saved catalogue.
