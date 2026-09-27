# ModelTest

Verifies WMM2025 and IGRF14 against direct GeographicLib calls, east-north-up/nanotesla boundary conversion to north-east-down/tesla, derived intensities and angles, provenance, hashes, UTC and depth validity, atomic batch limits, and concurrent singleton evaluation.

Run `dotnet test ModelTest/ModelTest.csproj`.

Author: Eric Cayeux

Company: NORCE Research

Magnetic-dip checks verify the `MagneticDip` calculation and contract name; service tests cover REST, MCP, OpenAPI and the generated client, including removal of the former result property.
