using OSDC.DotnetLibraries.Drilling.SemanticCatalogue;
namespace OSDC.Drilling.EarthMagneticField.Model;

/// <summary>Identity, validity, and provenance of one installed geomagnetic reference model.</summary>
[Semantic(Concepts.GeomagneticModelProvenance)]
public class EarthMagneticModelInfo
{
    /// <summary>Provider model-selection token, distinct from the scientific model identifier.</summary>
    [Semantic(Concepts.ModelSelectionToken)]
    public EarthMagneticFieldModel Model { get; set; }
    /// <summary>Name of the installed scientific model.</summary>
    [Semantic(Concepts.ModelName)]
    public string Name { get; set; } = string.Empty;
    /// <summary>Scientific model identifier, distinct from the provider selection token.</summary>
    [Semantic(Concepts.ModelId)]
    public string ID { get; set; } = string.Empty;
    /// <summary>Human-readable description of the installed model.</summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>Publication calendar date, serialized as a nullable date-time; not a UTC evaluation instant.</summary>
    [Semantic(Concepts.ReleaseDate)]
    public DateTime? ReleaseDate { get; set; }
    /// <summary>Inclusive lower bound of the supported evaluation-time domain, in UTC.</summary>
    [Semantic(Concepts.Instant, Role = Concepts.LowerBound, Reference = Concepts.Utc)]
    public DateTimeOffset MinimumUtc { get; set; }
    /// <summary>Inclusive upper bound of the supported evaluation-time domain, in UTC.</summary>
    [Semantic(Concepts.Instant, Role = Concepts.UpperBound, Reference = Concepts.Utc)]
    public DateTimeOffset MaximumUtc { get; set; }
    /// <summary>Inclusive lower bound of supported ellipsoidal depth in SI metres, positive down from WGS84.</summary>
    [Semantic(Concepts.EllipsoidalDepth, Role = Concepts.LowerBound, Reference = Concepts.Wgs84)]
    public double MinimumDepth { get; set; }
    /// <summary>Inclusive upper bound of supported ellipsoidal depth in SI metres, positive down from WGS84.</summary>
    [Semantic(Concepts.EllipsoidalDepth, Role = Concepts.UpperBound, Reference = Concepts.Wgs84)]
    public double MaximumDepth { get; set; }
    /// <summary>Maximum spherical-harmonic degree represented by the model; not an angle.</summary>
    [Semantic(Concepts.HarmonicDegree)]
    public int Degree { get; set; }
    /// <summary>Maximum spherical-harmonic order represented by the model; not an angle.</summary>
    [Semantic(Concepts.HarmonicOrder)]
    public int Order { get; set; }
    /// <summary>Version of the calculation implementation used for reproducibility.</summary>
    [Semantic(Concepts.RuntimeVersion)]
    public string GeographicLibVersion { get; set; } = string.Empty;
    /// <summary>Reference ellipsoid used for the geodetic position.</summary>
    [Semantic(Concepts.ReferenceEllipsoid)]
    public string ReferenceEllipsoid { get; set; } = "WGS84";
    /// <summary>Local north-east-down frame; down is opposite ellipsoid-normal up.</summary>
    public string CoordinateFrame { get; set; } = "north-east-down";
    /// <summary>Wire unit for magnetic flux density: tesla, not magnetic field strength in amperes per metre.</summary>
    public string MagneticFluxDensityUnit { get; set; } = "tesla";
    /// <summary>Wire unit for angles: radian.</summary>
    public string AngleUnit { get; set; } = "radian";
    /// <summary>Depth increases downward from the reference ellipsoid.</summary>
    public string DepthPositiveDirection { get; set; } = "down";
    /// <summary>Whether the installed evaluator supports concurrent evaluations.</summary>
    public bool ConcurrentEvaluationEnabled { get; set; } = true;
    /// <summary>SHA-256 digest of the complete model metadata file bytes, encoded as 64 lowercase hexadecimal characters.</summary>
    [Semantic(Concepts.Sha256FileDigest, Role = Concepts.ModelMetadataFile)]
    public string MetadataSHA256 { get; set; } = string.Empty;
    /// <summary>SHA-256 digest of the complete coefficient file bytes, encoded as 64 lowercase hexadecimal characters.</summary>
    [Semantic(Concepts.Sha256FileDigest, Role = Concepts.CoefficientFile)]
    public string CoefficientSHA256 { get; set; } = string.Empty;
}
