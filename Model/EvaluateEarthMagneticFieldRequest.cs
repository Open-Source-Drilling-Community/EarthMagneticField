using OSDC.DotnetLibraries.Drilling.SemanticCatalogue;
using System.ComponentModel.DataAnnotations;

namespace OSDC.Drilling.EarthMagneticField.Model;

/// <summary>A stateless synchronous geomagnetic-field evaluation request.</summary>
[Semantic(Concepts.GeomagneticRequest)]
public class EvaluateEarthMagneticFieldRequest
{
    /// <summary>Reference model used for every sample in this batch. REST defaults omission to WMM2025; MCP requires explicit selection.</summary>
    [Semantic(Concepts.ModelSelectionToken)]
    public EarthMagneticFieldModel Model { get; set; } = EarthMagneticFieldModel.WMM2025;

    /// <summary>Evaluation points. Any invalid sample rejects the complete request.</summary>
    [Required, MinLength(1)]
    [Semantic(Concepts.GeodeticEvaluationPoint, Role = Concepts.InputEvaluationPoints)]
    public List<EarthMagneticFieldEvaluationPoint> Samples { get; set; } = [];
}
