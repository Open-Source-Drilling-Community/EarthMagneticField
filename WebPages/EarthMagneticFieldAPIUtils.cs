using NORCE.Drilling.EarthMagneticField.ModelShared;
using OSDC.DotnetLibraries.Drilling.WebAppUtils;

namespace NORCE.Drilling.EarthMagneticField.WebPages;

public class EarthMagneticFieldAPIUtils : APIUtils, IEarthMagneticFieldAPIUtils
{
    public EarthMagneticFieldAPIUtils(IEarthMagneticFieldWebPagesConfiguration configuration)
    {
        HostNameEarthMagneticField = Require(configuration.EarthMagneticFieldHostURL, nameof(configuration.EarthMagneticFieldHostURL));
        HttpClientEarthMagneticField = SetHttpClient(HostNameEarthMagneticField, HostBasePathEarthMagneticField);
        ClientEarthMagneticField = new Client(HttpClientEarthMagneticField.BaseAddress!.ToString(), HttpClientEarthMagneticField);

        HostNameUnitConversion = Require(configuration.UnitConversionHostURL, nameof(configuration.UnitConversionHostURL));
    }

    private static string Require(string? value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Configuration value '{propertyName}' must be assigned before WebPages is used.");
        }

        return value;
    }

    public string HostNameEarthMagneticField { get; }
    public string HostBasePathEarthMagneticField { get; } = "EarthMagneticField/api/";
    public HttpClient HttpClientEarthMagneticField { get; }
    public Client ClientEarthMagneticField { get; }

    public string HostNameUnitConversion { get; }
    public string HostBasePathUnitConversion { get; } = "UnitConversion/api/";
}
