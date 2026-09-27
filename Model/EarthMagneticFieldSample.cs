using OSDC.DotnetLibraries.Drilling.SemanticCatalogue;
namespace OSDC.Drilling.EarthMagneticField.Model;

/// <summary>A geomagnetic-field result in the public north-east-down frame.</summary>
[Semantic(Concepts.GeomagneticSample)]
public class EarthMagneticFieldSample
{
    /// <summary>The validated input, including its UTC evaluation instant.</summary>
    [Semantic(Concepts.GeodeticEvaluationPoint)]
    public EarthMagneticFieldEvaluationPoint Input { get; set; } = new();

    /// <summary>Northerly magnetic-flux-density component in SI teslas.</summary>
    [Semantic(Concepts.EarthMagneticFluxDensity, Role = Concepts.North, Reference = Concepts.Ned)]
    public double North { get; set; }

    /// <summary>Easterly magnetic-flux-density component in SI teslas.</summary>
    [Semantic(Concepts.EarthMagneticFluxDensity, Role = Concepts.East, Reference = Concepts.Ned)]
    public double East { get; set; }

    /// <summary>Downward magnetic-flux-density component in SI teslas.</summary>
    [Semantic(Concepts.EarthMagneticFluxDensity, Role = Concepts.Down, Reference = Concepts.Ned)]
    public double Down { get; set; }

    /// <summary>Horizontal magnetic-flux-density magnitude in SI teslas.</summary>
    [Semantic(Concepts.EarthMagneticFluxDensity, Role = Concepts.HorizontalMagnitude, Reference = Concepts.Ned)]
    public double HorizontalIntensity { get; set; }

    /// <summary>Total magnetic-flux-density magnitude in SI teslas.</summary>
    [Semantic(Concepts.EarthMagneticFluxDensity, Role = Concepts.Magnitude)]
    public double TotalIntensity { get; set; }

    /// <summary>Declination in SI radians, positive east of geodetic north; null when horizontal intensity is zero.</summary>
    [Semantic(Concepts.MagneticDeclination, Reference = Concepts.Ned)]
    public double? Declination { get; set; }

    /// <summary>Magnetic dip in SI radians, positive downward from horizontal; null when total intensity is zero.</summary>
    [Semantic(Concepts.MagneticDip, Reference = Concepts.Ned)]
    public double? MagneticDip { get; set; }
}
