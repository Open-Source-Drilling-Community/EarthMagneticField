namespace NORCE.Drilling.EarthMagneticField.ModelShared
{
	public class PseudoConstructors
	{
		public static MetaInfo ConstructMetaInfo()
			{
				return new MetaInfo 
				{
					ID = Guid.NewGuid(),
					HttpHostName = "https://dev.digiwells.no/",
					HttpHostBasePath = "EarthMagneticField/api/",
					HttpEndPoint = "EarthMagneticFieldCalculationOrder/",
				};
			}

		public static MetaInfo ConstructMetaInfo(Guid id)
			{
				return new MetaInfo 
				{
					ID = id,
					HttpHostName = "https://dev.digiwells.no/",
					HttpHostBasePath = "EarthMagneticField/api/",
					HttpEndPoint = "EarthMagneticFieldCalculationOrder/",
				};
			}
		public static EarthMagneticData ConstructEarthMagneticData()
		{
			return new EarthMagneticData
			{
				Latitude = 0.0, 
				Longitude = 0.0, 
				Depth = 0.0, 
				Year = 2025.0, 
				Dip = null, 
				FieldIntensity = null, 
				Declination = null, 
				HorizontalMagneticField = null, 
			};
		}
		public static EarthMagneticField ConstructEarthMagneticField()
		{
			return new EarthMagneticField
			{
				MetaInfo = ConstructMetaInfo(),
				Name = "Default Name",
				Description = "Default Description",
				CreationDate = DateTimeOffset.UtcNow,
				LastModificationDate = DateTimeOffset.UtcNow,
				EarthMagneticFieldData = new List<EarthMagneticData>
					{
						ConstructEarthMagneticData(),
					},
				Type = (EarthMagneticFieldType)0,
			};
		}
		public static EarthMagneticFieldCalculationOrder ConstructEarthMagneticFieldCalculationOrder()
		{
			return new EarthMagneticFieldCalculationOrder
			{
				MetaInfo = ConstructMetaInfo(),
				Name = "Default Name",
				Description = "Default Description",
				CreationDate = DateTimeOffset.UtcNow,
				LastModificationDate = DateTimeOffset.UtcNow,
				CalculationMethod = (EarthMagneticFieldCalculationMethod)0,
				RawEarthMagneticFieldTable = ConstructEarthMagneticField(),
				CompletedEarthMagneticFieldTable = ConstructEarthMagneticField(),
			};
		}
	}
}