using System.Text.Json.Nodes;
using OSDC.Drilling.EarthMagneticField.Model;
using OSDC.DotnetLibraries.Drilling.SemanticCatalogue;

namespace OSDC.Drilling.EarthMagneticField.Service.Mcp.Tools;

internal static class EarthMagneticFieldMcpSchemas
{
    public static JsonNode EvaluateInput(int maximumSamples)
    {
        var schema = (JsonObject)BuildEvaluateInput(maximumSamples);
        SemanticMetadata.AnnotateObject(schema, typeof(EvaluateEarthMagneticFieldRequest));
        schema[SemanticMetadata.ExtensionName]!["role"] = Concepts.StatelessEvaluation;
        SemanticMetadata.AnnotateObject((JsonObject)schema["properties"]!["Samples"]!["items"]!, typeof(EarthMagneticFieldEvaluationPoint));
        return schema;
    }

    public static JsonNode EvaluateOutput()
    {
        var schema = (JsonObject)BuildEvaluateOutput();
        SemanticMetadata.AnnotateObject(schema, typeof(EvaluateEarthMagneticFieldResponse));
        SemanticMetadata.AnnotateObject((JsonObject)schema["$defs"]!["input"]!, typeof(EarthMagneticFieldEvaluationPoint));
        SemanticMetadata.AnnotateObject((JsonObject)schema["$defs"]!["result"]!, typeof(EarthMagneticFieldSample));
        SemanticMetadata.AnnotateObject((JsonObject)schema["$defs"]!["modelInfo"]!, typeof(EarthMagneticModelInfo));
        return schema;
    }

    public static JsonNode ServiceInfo()
    {
        var schema = (JsonObject)BuildServiceInfo();
        // Use exactly the same model schema and annotations for discovery and evaluation.
        schema["$defs"]!["modelInfo"] = EvaluateOutput()["$defs"]!["modelInfo"]!.DeepClone();
        SemanticMetadata.AnnotateObject(schema, typeof(EarthMagneticFieldServiceInfo));
        return schema;
    }

    public static JsonNode ServiceInfoInput()
    {
        var schema = (JsonObject)JsonNode.Parse("""{"type":"object","properties":{},"additionalProperties":false}""")!;
        schema[SemanticMetadata.ExtensionName] = SemanticMetadata.Create(Concepts.GeomagneticModelProvenance, Concepts.ResourceCollectionRetrieval, assertionSource: "provider-mcp-operation");
        return schema;
    }

    private static JsonNode BuildEvaluateInput(int maximumSamples) => JsonNode.Parse($$"""
    {
      "type":"object",
      "properties":{
        "Model":{"type":"string","enum":["WMM2025","IGRF14"],"description":"Reference model for the complete batch. WMM2025 covers 2025-2030; IGRF14 covers 1900-2030."},
        "Samples":{
          "type":"array","minItems":1,"maxItems":{{maximumSamples}},
          "description":"Independent points evaluated in order. One invalid point rejects the complete batch.",
          "items":{
            "type":"object",
            "properties":{
              "Latitude":{"type":"number","minimum":-1.5707963267948966,"maximum":1.5707963267948966,"description":"WGS84 geodetic latitude in SI radians; never degrees."},
              "Longitude":{"type":"number","minimum":-3.141592653589793,"maximum":3.141592653589793,"description":"WGS84 longitude in SI radians; never degrees."},
              "Depth":{"type":"number","description":"SI metres positive downward from the WGS84 ellipsoid. Model-dependent bounds apply."},
              "DateTimeUtc":{"type":"string","format":"date-time","description":"UTC instant containing Z or +00:00. Other offsets and unspecified local times are rejected."}
            },
            "required":["Latitude","Longitude","Depth","DateTimeUtc"],
            "additionalProperties":false
          }
        }
      },
      "required":["Model","Samples"],
      "examples":[{"Model":"WMM2025","Samples":[{"Latitude":1.0471975511965976,"Longitude":0.17453292519943295,"Depth":1000.0,"DateTimeUtc":"2026-08-24T10:00:00Z"}]}],
      "additionalProperties":false
    }
    """)!;

    private static JsonNode BuildEvaluateOutput() => JsonNode.Parse("""
    {
      "type":"object",
      "properties":{
        "Model":{"$ref":"#/$defs/modelInfo"},
        "Samples":{"type":"array","items":{"$ref":"#/$defs/result"}}
      },
      "required":["Model","Samples"],
      "additionalProperties":false,
      "$defs":{
        "input":{
          "type":"object",
          "properties":{
            "Latitude":{"type":"number","description":"WGS84 latitude in SI radians."},
            "Longitude":{"type":"number","description":"WGS84 longitude in SI radians."},
            "Depth":{"type":"number","description":"SI metres positive downward from the WGS84 ellipsoid."},
            "DateTimeUtc":{"type":"string","format":"date-time","description":"Normalized UTC evaluation instant."}
          },
          "required":["Latitude","Longitude","Depth","DateTimeUtc"],"additionalProperties":false
        },
        "result":{
          "type":"object",
          "properties":{
            "Input":{"$ref":"#/$defs/input"},
            "North":{"type":"number","description":"Northerly component in SI teslas."},
            "East":{"type":"number","description":"Easterly component in SI teslas."},
            "Down":{"type":"number","description":"Downward component in SI teslas."},
            "HorizontalIntensity":{"type":"number","minimum":0,"description":"Horizontal magnitude in SI teslas."},
            "TotalIntensity":{"type":"number","minimum":0,"description":"Total magnitude in SI teslas."},
            "Declination":{"type":["number","null"],"description":"SI radians positive east of north; null if horizontal intensity is zero."},
            "MagneticDip":{"type":["number","null"],"description":"SI radians positive downward; null if total intensity is zero."}
          },
          "required":["Input","North","East","Down","HorizontalIntensity","TotalIntensity","Declination","MagneticDip"],"additionalProperties":false
        },
        "modelInfo":{
          "type":"object",
          "properties":{
            "Model":{"description":"Provider model-selection token, distinct from the scientific model identifier.","type":"string","enum":["WMM2025","IGRF14"]},"Name":{"description":"Name of the installed scientific model.","type":"string"},"ID":{"description":"Scientific model identifier, distinct from the provider selection token.","type":"string"},"Description":{"description":"Human-readable description of the installed model.","type":"string"},
            "ReleaseDate":{"description":"Publication calendar date, serialized as a nullable date-time; not a UTC evaluation instant.","type":["string","null"],"format":"date-time"},"MinimumUtc":{"description":"Inclusive lower bound of the supported evaluation-time domain, in UTC.","type":"string","format":"date-time"},"MaximumUtc":{"description":"Inclusive upper bound of the supported evaluation-time domain, in UTC.","type":"string","format":"date-time"},
            "MinimumDepth":{"description":"Inclusive lower bound of supported ellipsoidal depth in SI metres, positive down from WGS84.","type":"number"},"MaximumDepth":{"description":"Inclusive upper bound of supported ellipsoidal depth in SI metres, positive down from WGS84.","type":"number"},"Degree":{"description":"Maximum spherical-harmonic degree represented by the model; not an angle.","type":"integer"},"Order":{"description":"Maximum spherical-harmonic order represented by the model; not an angle.","type":"integer"},"GeographicLibVersion":{"description":"Version of the calculation implementation used for reproducibility.","type":"string"},
            "ReferenceEllipsoid":{"description":"Reference ellipsoid used for the geodetic position.","type":"string","const":"WGS84"},"CoordinateFrame":{"description":"Local north-east-down frame; down is opposite ellipsoid-normal up.","type":"string","const":"north-east-down"},"MagneticFluxDensityUnit":{"description":"Wire unit for magnetic flux density: tesla, not magnetic field strength in amperes per metre.","type":"string","const":"tesla"},
            "AngleUnit":{"description":"Wire unit for angles: radian.","type":"string","const":"radian"},"DepthPositiveDirection":{"description":"Depth increases downward from the reference ellipsoid.","type":"string","const":"down"},"ConcurrentEvaluationEnabled":{"description":"Whether the installed evaluator supports concurrent evaluations.","type":"boolean"},
            "MetadataSHA256":{"description":"SHA-256 digest of the complete model metadata file bytes, encoded as 64 lowercase hexadecimal characters.","type":"string","pattern":"^[0-9a-f]{64}$"},"CoefficientSHA256":{"description":"SHA-256 digest of the complete coefficient file bytes, encoded as 64 lowercase hexadecimal characters.","type":"string","pattern":"^[0-9a-f]{64}$"}
          },
          "required":["Model","Name","ID","Description","ReleaseDate","MinimumUtc","MaximumUtc","MinimumDepth","MaximumDepth","Degree","Order","GeographicLibVersion","ReferenceEllipsoid","CoordinateFrame","MagneticFluxDensityUnit","AngleUnit","DepthPositiveDirection","ConcurrentEvaluationEnabled","MetadataSHA256","CoefficientSHA256"],
          "additionalProperties":false
        }
      }
    }
    """)!;

    private static JsonNode BuildServiceInfo() => JsonNode.Parse("""
    {
      "type":"object",
      "properties":{
        "Name":{"type":"string","const":"OSDC Earth Magnetic Field"},
        "Description":{"type":"string"},
        "CoordinateFrame":{"type":"string","const":"north-east-down"},
        "TimeConvention":{"type":"string","const":"UTC"},
        "DepthReference":{"type":"string","const":"WGS84 reference ellipsoid"},
        "DepthPositiveDirection":{"type":"string","const":"down"},
        "Models":{"type":"array","minItems":2,"items":{"$ref":"#/$defs/modelInfo"}}
      },
      "required":["Name","Description","CoordinateFrame","TimeConvention","DepthReference","DepthPositiveDirection","Models"],
      "additionalProperties":false,
      "$defs":{}
    }
    """)!;
}
