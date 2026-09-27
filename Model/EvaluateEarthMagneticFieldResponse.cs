using OSDC.DotnetLibraries.Drilling.SemanticCatalogue;
namespace OSDC.Drilling.EarthMagneticField.Model;

/// <summary>Geomagnetic results in the same order as the request samples.</summary>
[Semantic(Concepts.GeomagneticResponse)]
public class EvaluateEarthMagneticFieldResponse
{
    [Semantic(Concepts.GeomagneticModelProvenance, Role = Concepts.Provenance)]
    public EarthMagneticModelInfo Model { get; set; } = new();
    [Semantic(Concepts.GeomagneticSample, Role = Concepts.OutputSamples)]
    public List<EarthMagneticFieldSample> Samples { get; set; } = [];
}
