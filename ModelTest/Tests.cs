using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.DotnetLibraries.General.DataManagement;
using OSDC.DotnetLibraries.General.Statistics;
using NORCE.Drilling.EarthMagneticField.Model;

namespace NORCE.Drilling.EarthMagneticField.ModelTest
{
    public class Tests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }

        [Test]
        public void Test_Calculus()
        {
            Guid guid = Guid.NewGuid();
            MetaInfo metaInfo = new() { ID = guid };
            DateTimeOffset creationDate = DateTimeOffset.UtcNow;

            Guid guid2 = Guid.NewGuid();
            MetaInfo metaInfo2 = new() { ID = guid2 };
            DateTimeOffset creationDate2 = DateTimeOffset.UtcNow;
            List<EarthMagneticData> data = new List<EarthMagneticData>
            {
                new EarthMagneticData
                {
                    Latitude = 0,
                    Longitude = 0,
                    Depth = 0,
                    Year = 2025
                }
            };  
            Model.EarthMagneticField earthMagneticField = new()
            {
                MetaInfo = metaInfo2,
                Name = "My test EarthMagneticField name",
                Description = "My test EarthMagneticField for POST",
                CreationDate = creationDate,
                LastModificationDate = creationDate2,
                Type = EarthMagneticFieldType.Raw,
                EarthMagneticFieldData = data
            };
            Model.EarthMagneticFieldCalculationOrder earthMagneticFieldCalculationOrder = new()
            {
                MetaInfo = metaInfo,
                Name = "My test EarthMagneticFieldCalculationOrder",
                Description = "My test EarthMagneticFieldCalculationOrder",
                CreationDate = creationDate,
                LastModificationDate = creationDate,
                RawEarthMagneticFieldTable = earthMagneticField,
                CalculationMethod = EarthMagneticFieldCalculationMethod.WMM2025
            };

            bool success = earthMagneticFieldCalculationOrder.Calculate();
            Assert.That(success == true);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }
    }
}