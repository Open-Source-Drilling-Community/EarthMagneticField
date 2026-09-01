# OSDC.Drilling.EarthMagneticField.WebPages

Reusable Blazor pages for stateless Earth magnetic-field evaluation:

- `/EarthMagneticFieldCalculation`: unit-aware WMM2025/IGRF14 evaluation with explicit UTC and north-east-down output.
- `/EarthMagneticFieldModel`: installed-model validity and provenance.
- `/StatisticsEarthMagneticField`: cumulative operational counters retained by the service's persistent data volume.

Consumers implement and register `IEarthMagneticFieldWebPagesConfiguration`, register `IEarthMagneticFieldAPIUtils` with `APIUtils`, add MudBlazor and the OSDC unit-system services, and include the WebPages assembly in the Blazor router. `APIUtils` creates the generated client from the configured host root and appends `EarthMagneticField/api/`.

Package ID: `OSDC.Drilling.EarthMagneticField.WebPages`

Author: Eric Cayeux

Company: NORCE Research
