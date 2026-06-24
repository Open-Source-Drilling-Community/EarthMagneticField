using NORCE.Drilling.EarthMagneticField.WebPages;

namespace NORCE.Drilling.EarthMagneticField.WebApp;

public class WebPagesHostConfiguration : IEarthMagneticFieldWebPagesConfiguration
{
    public string EarthMagneticFieldHostURL { get; set; } = string.Empty;
    public string? UnitConversionHostURL { get; set; } = string.Empty;
}
