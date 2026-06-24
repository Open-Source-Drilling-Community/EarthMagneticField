using OSDC.DotnetLibraries.General.DataManagement;
using System;
using System.Collections.Generic;
namespace NORCE.Drilling.EarthMagneticField.Model
{
    /// <summary>
    /// a base class other classes may derive from
    /// </summary>
    public class EarthMagneticField
    {
        /// <summary>
        /// a MetaInfo for the EarthMagneticField
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

        /// <summary>
        /// a parameter defined as a Gaussian distribution 
        /// </summary>
        public List<EarthMagneticData> EarthMagneticFieldData { get; set; }

        /// <summary>
        /// the type of the "derived" data instanciated (supposedly unique)
        /// </summary>
        public EarthMagneticFieldType Type { get; set; }


        /// <summary>
        /// default constructor required for JSON serialization
        /// </summary>
        public EarthMagneticField() : base()
        {
        }
    }
}
