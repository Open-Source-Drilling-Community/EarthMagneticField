using OSDC.DotnetLibraries.Drilling.SemanticCatalogue;
using System.ComponentModel.DataAnnotations;

namespace OSDC.Drilling.EarthMagneticField.Model;

/// <summary>A WGS84 position and UTC instant at which to evaluate the magnetic field.</summary>
[Semantic(Concepts.GeodeticEvaluationPoint)]
public class EarthMagneticFieldEvaluationPoint
{
    /// <summary>WGS84 geodetic latitude in SI radians, between -π/2 and π/2.</summary>
    [Range(-1.5707963267948966, 1.5707963267948966)]
    [Semantic(Concepts.Latitude, Reference = Concepts.Wgs84)]
    public double Latitude { get; set; }

    /// <summary>WGS84 geodetic longitude in SI radians, between -π and π.</summary>
    [Range(-3.141592653589793, 3.141592653589793)]
    [Semantic(Concepts.Longitude, Reference = Concepts.Wgs84)]
    public double Longitude { get; set; }

    /// <summary>Depth in SI metres, positive downward from the WGS84 reference ellipsoid.</summary>
    [Semantic(Concepts.EllipsoidalDepth, Reference = Concepts.Wgs84)]
    public double Depth { get; set; }

    /// <summary>UTC evaluation instant. JSON input must contain Z or the equivalent +00:00 offset.</summary>
    [Required]
    [Semantic(Concepts.Instant, Reference = Concepts.Utc)]
    public DateTimeOffset DateTimeUtc { get; set; }
}
