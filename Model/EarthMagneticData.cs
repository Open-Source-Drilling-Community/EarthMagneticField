using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.DotnetLibraries.General.DataManagement;
using System;

namespace NORCE.Drilling.EarthMagneticField.Model
{
    /// <summary>
    /// a base class other classes may derive from
    /// </summary>
    public class EarthMagneticData
    {
        /// <summary>
        /// Double with Lattitude in SI Units
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Double with Longitude in SI Units
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Double with Depth in SI Units
        /// </summary>
        public double Depth { get; set; }
        /// <summary>
        /// Double with Year in SI Units
        /// </summary>
        public double Year { get; set; }
        
        /// <summary>
        /// Nullable double with Dip in SI Units
        /// </summary>
        public double? Dip { get; set; }
        /// <summary>
        /// Nullable double with Magnetic field intensity in SI Units
        /// </summary>
        public double? FieldIntensity { get; set; }
        /// <summary>
        /// Nullable double with Declination in SI Units
        /// </summary>
        public double? Declination { get; set; }
        /// <summary>
        /// Nullable double with Horizontal magnetic fiels in SI Units
        /// </summary>
        public double? HorizontalMagneticField { get; set; }

       
        /// <summary>
        /// default constructor required for JSON serialization
        /// </summary>
        public EarthMagneticData() : base()
        {
        }
    }
}
