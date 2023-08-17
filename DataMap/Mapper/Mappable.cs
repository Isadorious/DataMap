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
		//private Type _mapsTo = typeof(T);
		//private Type? _mapsFrom;
		//private bool? _isReversable;
		//private Dictionary<MemberInfo, MemberInfo>? _attributeMapping;

		//private void SetUp()
		//{
		//	if (_mapsFrom == null) GetMapsToType();
		//	if (_isReversable == null) GetIsReversableMapping();
		//	if (_attributeMapping == null) GetPropertyMapping();
		//}

		private static MappingData GetMappingData()
		{
			if(!MappingDataSingleton.Instance.TypeMaps.ContainsKey(typeof(T)))
			{
                MappingDataSingleton.Instance.TypeMaps.Add(typeof(T), new MappingData(typeof(T)));
            }

            return MappingDataSingleton.Instance.TypeMaps.Single(x => x.Key == typeof(T)).Value;
        }

  //      private void GetMapsToType()
		//{
  //          MapFromAttribute mapsFrom = (MapFromAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(MapFromAttribute));

  //          if (mapsFrom == null || mapsFrom.GetTargetType == null)
		//	{
		//		throw new ArgumentNullException("No target type specified");
		//	}

		//	_mapsFrom = mapsFrom.GetTargetType;
  //      }

		//private void GetIsReversableMapping()
		//{
		//	MapReversibleAttribute mapReversibleAttribute = (MapReversibleAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(MapReversibleAttribute));

		//	if (mapReversibleAttribute == null) _isReversable = false;
		//	else _isReversable = true;
		//}

		//private void GetPropertyMapping()
		//{
		//	if (_mapsFrom == null) throw new NullReferenceException("Provide type mapping is null - cannot proceed with mapping");
  //          MemberInfo[] PropertyInfos = _mapsTo.GetProperties();

  //          for (int i = 0; i < PropertyInfos.Length; i++)
  //          {
  //              var attribute = (MapFromAttribute)Attribute.GetCustomAttribute(PropertyInfos[i], typeof(MapFromAttribute));

  //              if (attribute == null) continue; // no attribute on this property

  //              if (attribute.GetTargetFieldName == null)
  //              {
  //                  throw new ArgumentNullException($"No target field name specified for {PropertyInfos[i].Name}");
  //              }

		//		// store the list of mappable properties
		//		var originMember = _mapsFrom.GetProperty(attribute.GetTargetFieldName);

		//		if (originMember == null)
		//		{
		//			throw new ArgumentException($"Unable to find property with name {attribute.GetTargetFieldName} on type {_mapsFrom.Name}");
		//		}

		//		if(_attributeMapping == null)
		//		{
		//			_attributeMapping = new();
		//		}

		//		if (_attributeMapping.ContainsKey(originMember))
		//		{
		//			throw new ArgumentException($"Unable to map {attribute.GetTargetFieldName} to multiple properties");
		//		}

		//		_attributeMapping.Add(originMember, PropertyInfos[i]);
  //          }
		//}

		public T Map<U>(U mapFrom)
		{
			//SetUp();

			var mappingData = GetMappingData();
			var _mapsFrom = mappingData.MapsFrom;
			var _mapsTo = mappingData.MapsTo;
			var _attributeMapping = mappingData.AttributeMapping;

			if(typeof(U) != _mapsFrom)
			{
				throw new ArgumentException($"Cannot map between {typeof(U)} and {typeof(T)} please use a {_mapsFrom?.Name} type instead");
			}

			if(mapFrom == null)
			{
				throw new ArgumentNullException($"Cannot map a null object");
			}
			// using the information contained within attributeMapping convert U to T
			// iterate over the properties in T then set the properties from the content in U if a mapping exists
			// no mapping setup = no copy
			object toRet = Activator.CreateInstance(_mapsTo);
			dynamic toMapDynamic = mapFrom;

			MemberInfo[] propertyList = _mapsTo.GetProperties();

			foreach(var targetProperty in propertyList)
			{
				if (targetProperty == null) continue;

				// get origin property from attribute mapping dictionary
				var originProperty = _attributeMapping.FirstOrDefault(x => x.Value.Name == targetProperty.Name && x.Value.MemberType == targetProperty.MemberType).Key;

				if(originProperty == null)
				{
					throw new ArgumentException($"Unable to find origin property for {targetProperty.Name}");
				}

				PropertyInfo originPropInfo = _mapsFrom.GetProperty(originProperty.Name);
				PropertyInfo targetPropInfo = _mapsTo.GetProperty(targetProperty.Name);

				var originValue = originPropInfo.GetValue(mapFrom);

				targetPropInfo.SetValue(toRet, originValue);
				targetPropInfo.SetValue(this, originValue);
			}

			return toRet as T;
		}

        public U ReverseMap<U>() where U : class
		{
            //SetUp();

            var mappingData = GetMappingData();
            var _mapsFrom = mappingData.MapsFrom;
            var _mapsTo = mappingData.MapsTo;
            var _attributeMapping = mappingData.AttributeMapping;
			var _isReversable = mappingData.IsRevserable;

            if (_isReversable != true)
			{
				throw new ArgumentException($"Reverse mapping not enabled for type {typeof(T)}");
			}

            if (typeof(U) != _mapsFrom)
            {
                throw new ArgumentException($"Cannot map between {typeof(U)} and {typeof(T)} please use a {_mapsFrom?.Name} type instead");
            }
            // using the information contained within attributeMapping convert T to U
            // iterate over the properties in U then set the properties from the content in T if a mapping exists
            // no mapping setup = no copy
            object toRet = Activator.CreateInstance(_mapsFrom);
            MemberInfo[] propertyList = _mapsFrom.GetProperties();

            foreach (var targetProperty in propertyList)
            {
				if (targetProperty == null) continue;

				// get origin property from attribute mapping directory
				var originProperty = _attributeMapping.FirstOrDefault(x => x.Key.Name == targetProperty.Name && x.Key.MemberType == targetProperty.MemberType).Value;

                if (originProperty == null)
                {
                    throw new ArgumentException($"Unable to find origin property for {targetProperty.Name}");
                }

				PropertyInfo originPropInfo = _mapsTo.GetProperty(originProperty.Name);
				PropertyInfo targetPropInfo = _mapsFrom.GetProperty(targetProperty.Name);

				var originValue = originPropInfo.GetValue(this);

				targetPropInfo.SetValue(toRet, originValue);

            }

            return toRet as U;
        }
    }
}

