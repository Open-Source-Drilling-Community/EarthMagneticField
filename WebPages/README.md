# OSDC.Drilling.EarthMagneticField.WebPages

This release targets MudBlazor 9.9.0 and the matching OSDC shared web component packages.

Reusable Blazor pages for stateless Earth magnetic-field evaluation:

- `/EarthMagneticFieldCalculation`: unit-aware WMM2025/IGRF14 evaluation with explicit UTC and north-east-down output.
- `/EarthMagneticFieldModel`: installed-model validity and provenance.
- `/StatisticsEarthMagneticField`: cumulative operational counters retained by the service's persistent data volume.

Consumers implement and register `IEarthMagneticFieldWebPagesConfiguration`, register `IEarthMagneticFieldAPIUtils` with `APIUtils`, add MudBlazor and the OSDC unit-system services, and include the WebPages assembly in the Blazor router. `APIUtils` creates the generated client from the configured host root and appends `EarthMagneticField/api/`.

Package ID: `OSDC.Drilling.EarthMagneticField.WebPages`

Author: Eric Cayeux

Company: NORCE Research

Magnetic dip uses the `MagneticDip` result property and the label “Magnetic dip”. It is in SI radians, positive downward from horizontal, and nullable when total magnetic flux density is zero. Service and client releases must agree on this property name.

HorizontalIntensity and TotalIntensity are displayed as horizontal/total magnetic flux density (EarthMagneticFluxDensity, SI teslas). MagneticDip and Declination use PlaneAngleDrilling in SI radians. Physical quantities agree with the service semantic annotations.
