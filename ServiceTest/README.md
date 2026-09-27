# ServiceTest

Hosts the actual ASP.NET Core service in memory and verifies both models through the generated client, structured HTTP 422 validation, discovery routes, operational endpoints, exact MCP registration and schemas, end-to-end MCP evaluation, exclusion of usage statistics from MCP, and usage-counter restoration across a service restart.

Run `dotnet test ServiceTest/ServiceTest.csproj`.

Author: Eric Cayeux

Company: NORCE Research

The merged OpenAPI document advertises the API root (`/EarthMagneticField/api`) as its server URL, excluding the schema route. Service tests verify that its advertised URL and operation path resolve to a working endpoint.

Magnetic-dip checks verify the `MagneticDip` calculation and contract name; service tests cover REST, MCP, OpenAPI and the generated client, including removal of the former result property.
