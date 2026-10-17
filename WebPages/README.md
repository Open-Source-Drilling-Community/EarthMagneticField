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

## Package release 1.0.4

This release packages the current WebPages and generated contracts. Update consuming WebApps to `OSDC.Drilling.EarthMagneticField.WebPages` version `1.0.4` after it is available on nuget.org.

## Trusted NuGet publishing

The `publish-webpages-nuget.yml` workflow authenticates with GitHub OIDC through `NuGet/login@v1`; no long-lived `NUGET_API_KEY` secret is used. Set `NUGET_USER` to the nuget.org profile username as a repository variable (a repository secret is also accepted). The publishing job requests `contents: read` and `id-token: write` and obtains its temporary credential immediately before uploading.

On nuget.org, the package owner must configure a trusted-publishing policy for `Open-Source-Drilling-Community/EarthMagneticField`, workflow filename `publish-webpages-nuget.yml`, with permission to push new versions of `OSDC.Drilling.EarthMagneticField.WebPages`. Leave the policy environment empty because the workflow does not declare a GitHub environment. Trigger the workflow manually with the intended version or push a `webpages-v*` tag. Already published versions are skipped.

## Package release 1.0.5

Requires OSDC.UnitConversion.DrillingRazorMudComponents 3.4.8 so embedded unit selectors consume bounded UnitSystem discovery pages. Rebuild consuming WebApps after upgrading this package.
