using OSDC.DotnetLibraries.General.DataManagement;
using System;
using GeographicLib;
using System.IO;
using System.Collections.Generic;

namespace NORCE.Drilling.EarthMagneticField.Model
{   


    public class EarthMagneticFieldCalculationOrder : EarthMagneticFieldCalculationOrderLight 
    {
        /// <summary>
        /// a MetaInfo for the EarthMagneticFieldCalculationOrder
        /// </summary>
        public MetaInfo? MetaInfo { get; set; }

        /// <summary>
        /// name of the data
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// a description of the data
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// the date when the data was created
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; }

        /// <summary>
        /// the date when the data was last modified
        /// </summary>
        public DateTimeOffset? LastModificationDate { get; set; }
    
        public EarthMagneticFieldCalculationMethod CalculationMethod { get; set; } = EarthMagneticFieldCalculationMethod.WMM2025;
        /// <summary>
        /// Table with raw data of EarthMagneticField
        /// </summary>
        public EarthMagneticField RawEarthMagneticFieldTable { get; set; }
        
        /// <summary>
        /// Table with raw data of EarthMagneticField
        /// </summary>
        public EarthMagneticField? CompletedEarthMagneticFieldTable { get; set; }
        
        /// <summary>
        /// default constructor required for JSON serialization
        /// </summary>
        public EarthMagneticFieldCalculationOrder() : base()
        {
        }

        /// <summary>
        /// main calculation method of the EarthMagneticFieldCalculationOrder
        /// </summary>
        /// <returns></returns>
        public bool Calculate()
        {
            bool success = false;
            if (RawEarthMagneticFieldTable.EarthMagneticFieldData.Count > 0)
            {
                // Get the propert path
                string magneticFieldModelPath;
                string baseDir = Path.Combine(AppContext.BaseDirectory, "MagneticFieldModelFiles");
                if (Directory.Exists(baseDir))
                {
                    magneticFieldModelPath = baseDir;
                }
                else
                {
                    DirectoryInfo? directory = new(Directory.GetCurrentDirectory());
                    while (directory != null && directory.GetFiles("*.sln").Length == 0)
                    {
                        directory = directory.Parent;
                    }
                    magneticFieldModelPath = Path.Combine(directory!.ToString(), "MagneticFieldModelFiles");
                }
                // Get the propert model
                string methodName = CalculationMethod switch
                {
                    EarthMagneticFieldCalculationMethod.IGRJ14 => "igrf14",
                    EarthMagneticFieldCalculationMethod.WMM2025 => "wmm2025",
                    _       => "Error" // The discard (_) acts as the default case
                };
                // leave if there is no proper selection
                if (methodName == "Error")
                {
                    success = false;
                    return success;
                }            
                success = true;
                List<EarthMagneticData> calculatedMagenticData = new();
                MagneticModel magneticModel = new MagneticModel(methodName, magneticFieldModelPath);
                foreach (EarthMagneticData earthMagneticData in RawEarthMagneticFieldTable.EarthMagneticFieldData)
                {
                    (double fieldX, double fieldY, double fieldZ) = magneticModel.Evaluate(
                                    earthMagneticData.Year, 
                                    earthMagneticData.Latitude, 
                                    earthMagneticData.Longitude, 
                                    - earthMagneticData.Depth,
                                    out double rateOfFieldX, 
                                    out double rateOfFieldY, 
                                    out double rateOfFieldZ
                                );
                   // Get relevant properties
                   (double horizontalMagneticField,
                    double totalMagneticField,
                    double declination,
                    double dip) = MagneticModel.FieldComponents(fieldX, fieldY, fieldZ, rateOfFieldX, rateOfFieldY, rateOfFieldZ);
                    // Converts to nano Tesla to Tesla
                    horizontalMagneticField /= 10E9;
                    totalMagneticField  /= 10E9;
                    dip = Math.PI*dip/180; // Converts to radians;
                    declination = Math.PI*declination/180; // Converts to radians;
                    calculatedMagenticData.Add(
                        new EarthMagneticData
                        {
                            Latitude = earthMagneticData.Latitude,
                            Longitude = earthMagneticData.Longitude,
                            Depth = earthMagneticData.Depth,
                            Year = earthMagneticData.Year,
                            Declination = declination,
                            Dip = dip,
                            FieldIntensity = totalMagneticField,
                            HorizontalMagneticField = horizontalMagneticField                            
                        }
                    );
                }
                MetaInfo metaInfo = new MetaInfo
                        {
                            ID = Guid.NewGuid(),
                            HttpEndPoint = MetaInfo!.HttpEndPoint,
                            HttpHostBasePath = MetaInfo!.HttpHostBasePath,
                            HttpHostName = MetaInfo!.HttpHostName                            
                        };
                // Format model string for creating the proper naming 
                string methodNameFormat = CalculationMethod switch
                {
                    EarthMagneticFieldCalculationMethod.IGRJ14 => "IGRF14",
                    EarthMagneticFieldCalculationMethod.WMM2025 => "WMM2025",
                    _       => "Error" // The discard (_) acts as the default case
                };
                CompletedEarthMagneticFieldTable = new EarthMagneticField
                {
                    MetaInfo = metaInfo,
                    Name = RawEarthMagneticFieldTable.Name + " (Completed)",
                    Description = $"Completed from ({methodNameFormat}): " + RawEarthMagneticFieldTable.Name,
                    CreationDate = CreationDate,
                    LastModificationDate = LastModificationDate,
                    Type = EarthMagneticFieldType.Completed,
                    EarthMagneticFieldData = calculatedMagenticData
                };
            }
            return success;
        }
    }
}
