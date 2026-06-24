using NORCE.Drilling.EarthMagneticField.ModelShared;

namespace NORCE.Drilling.EarthMagneticField.WebPages;

public interface IEarthMagneticFieldAPIUtils
{
    string HostNameEarthMagneticField { get; }
    string HostBasePathEarthMagneticField { get; }
    HttpClient HttpClientEarthMagneticField { get; }
    Client ClientEarthMagneticField { get; }

    string HostNameUnitConversion { get; }
    string HostBasePathUnitConversion { get; }
}
