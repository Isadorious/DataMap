using System;
using DataMap.Models;
namespace DataMap.Singleton
{
	internal sealed class MappingDataSingleton
	{
		private static readonly Lazy<MappingDataSingleton> lazySingleton = new Lazy<MappingDataSingleton>(() => new MappingDataSingleton());

		internal static MappingDataSingleton Instance { get { return lazySingleton.Value; } }

		internal Dictionary<Type, MappingData> TypeMaps { get; set; } = new Dictionary<Type, MappingData>();

		private MappingDataSingleton()
		{
		}
	}
}

