using OSDC.DotnetLibraries.Drilling.WebAppUtils;

namespace NORCE.Drilling.EarthMagneticField.WebPages;

public interface IEarthMagneticFieldWebPagesConfiguration :
    IUnitConversionHostURL
{
    string EarthMagneticFieldHostURL { get; }
}
