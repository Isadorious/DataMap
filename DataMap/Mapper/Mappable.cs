using System;
using System.Reflection;
using System.Xml.Serialization;
using DataMap.Atrributes;
using DataMap.Singleton;
using DataMap.Models;
namespace DataMap.Mapper
{
	public class Mappable<T> where T : class
	{
		private static MappingData GetMappingData()
		{
			if(!MappingDataSingleton.Instance.TypeMaps.ContainsKey(typeof(T)))
			{
                MappingDataSingleton.Instance.TypeMaps.Add(typeof(T), new MappingData(typeof(T)));
            }

            return MappingDataSingleton.Instance.TypeMaps.Single(x => x.Key == typeof(T)).Value;
        }

		public T Map<U>(U mapFrom)
		{
			var mappingData = GetMappingData();

			if(typeof(U) != mappingData.MapsFrom)
			{
				throw new ArgumentException($"Cannot map between {typeof(U)} and {typeof(T)} please use a {mappingData.MapsFrom?.Name} type instead");
			}

			if(mapFrom == null)
			{
				throw new ArgumentNullException($"Cannot map a null object");
			}
			// using the information contained within attributeMapping convert U to T
			// iterate over the properties in T then set the properties from the content in U if a mapping exists
			// no mapping setup = no copy
			object toRet = Activator.CreateInstance(mappingData.MapsTo);
			dynamic toMapDynamic = mapFrom;

			PropertyInfo[] propertyList = mappingData.MapsTo.GetProperties();

			foreach(var targetProperty in propertyList)
			{
				if (targetProperty == null) continue;

				// get origin property from attribute mapping dictionary
				var originProperty = mappingData.AttributeMapping.FirstOrDefault(x => x.Value.Name == targetProperty.Name && x.Value.MemberType == targetProperty.MemberType).Key;

				if(originProperty == null)
				{
					throw new ArgumentException($"Unable to find origin property for {targetProperty.Name}");
				}

				var originValue = originProperty.GetValue(mapFrom);

				targetProperty.SetValue(toRet, originValue);
				targetProperty.SetValue(this, originValue);
			}

			return toRet as T;
		}

        public U ReverseMap<U>() where U : class
		{
            var mappingData = GetMappingData();

            if (mappingData.IsRevserable != true)
			{
				throw new ArgumentException($"Reverse mapping not enabled for type {typeof(T)}");
			}

            if (typeof(U) != mappingData.MapsFrom)
            {
                throw new ArgumentException($"Cannot map between {typeof(U)} and {typeof(T)} please use a {mappingData.MapsFrom?.Name} type instead");
            }
            // using the information contained within attributeMapping convert T to U
            // iterate over the properties in U then set the properties from the content in T if a mapping exists
            // no mapping setup = no copy
            object toRet = Activator.CreateInstance(mappingData.MapsFrom);
            PropertyInfo[] propertyList = mappingData.MapsFrom.GetProperties();

            foreach (var targetProperty in propertyList)
            {
				if (targetProperty == null) continue;

				// get origin property from attribute mapping directory
				var originProperty = mappingData.AttributeMapping.FirstOrDefault(x => x.Key.Name == targetProperty.Name && x.Key.MemberType == targetProperty.MemberType).Value;

                if (originProperty == null)
                {
                    throw new ArgumentException($"Unable to find origin property for {targetProperty.Name}");
                }

				var originValue = originProperty.GetValue(this);

				targetProperty.SetValue(toRet, originValue);

            }

            return toRet as U;
        }
    }
}

