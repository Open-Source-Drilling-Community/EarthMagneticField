# NORCE.Drilling.EarthMagneticField.WebPages

Reusable Razor class library for the Earth Geomagnetic Field web UI.

It contains the `Home`, `EarthMagneticFieldSingleCalculation`, `EarthMagneticFieldCalculationOrderMain`, `EarthMagneticFieldCalculationOrderEdit`, and `EarthMagneticFieldView` pages together with the supporting API and UI utility code they depend on.

## Package contents

- Home page
- Earth geomagnetic field single calculation page
- Earth magnetic field calculation order list and edit pages
- Earth magnetic field result view page
- Host-configurable API access through injected configuration

## Dependencies

- `OSDC.DotnetLibraries.Drilling.WebAppUtils`
- `MudBlazor`
- `OSDC.UnitConversion.DrillingRazorMudComponents`
- `Plotly.Blazor`
- `ModelSharedOut`

## Host integration

The consuming app should:

1. Reference this package.
2. Provide an implementation of `IEarthMagneticFieldWebPagesConfiguration`.
3. Register that configuration and `IEarthMagneticFieldAPIUtils` in DI.
4. Add the `WebPages` assembly to the Blazor router `AdditionalAssemblies`.

## Required configuration

- `EarthMagneticFieldHostURL`
- `UnitConversionHostURL`

## Routes

- `/Home`
- `/EarthMagneticFieldSingleCalculation`
- `/EarthMagneticField`

# Funding

The current work has been funded by the [Research Council of Norway](https://www.forskningsradet.no/) and [Industry partners](https://www.digiwells.no/about/board/) in the framework of the cent for research-based innovation [SFI Digiwells (2020-2028)](https://www.digiwells.no/) focused on Digitalization, Drilling Engineering and GeoSteering. 

# Contributors

**Lucas Volpi**, *NORCE Energy Modelling and Automation*

**Eric Cayeux**, *NORCE Energy Modelling and Automation*
