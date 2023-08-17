using System;
using System.Reflection;
using DataMap.Atrributes;

namespace DataMap.Models
{
	internal class MappingData
	{

		internal MappingData(Type mapsTo)
		{
			MapsTo = mapsTo;
            GetMapsFromType();
			GetIsReversableMapping();
            GetPropertyMapping();
		}

		internal Type? MapsTo { get; set; }
		internal Type? MapsFrom { get; set; }
		internal bool? IsRevserable { get; set; }
		internal Dictionary<PropertyInfo, PropertyInfo>? AttributeMapping { get; set; }

        private void GetMapsFromType()
        {
            if (MapsTo == null) throw new NullReferenceException("Cannot map data to null");
            MapFromAttribute mapsFrom = (MapFromAttribute)Attribute.GetCustomAttribute(MapsTo, typeof(MapFromAttribute));

            if (mapsFrom == null || mapsFrom.GetTargetType == null)
            {
                throw new ArgumentNullException("No target type specified");
            }

            MapsFrom = mapsFrom.GetTargetType;
        }

        private void GetIsReversableMapping()
        {
            MapReversibleAttribute mapReversibleAttribute = (MapReversibleAttribute)Attribute.GetCustomAttribute(MapsTo, typeof(MapReversibleAttribute));

            IsRevserable = mapReversibleAttribute != null;
        }

        private void GetPropertyMapping()
        {
            if (MapsFrom == null) throw new NullReferenceException("Provided type mapping is null - cannot proceed with mapping");
            if (MapsTo == null) throw new NullReferenceException("Provided type mapping is null - cannot proceed with mapping");

            PropertyInfo[] PropertyInfos = MapsTo.GetProperties();

            for (int i = 0; i < PropertyInfos.Length; i++)
            {
                var attribute = (MapFromAttribute)Attribute.GetCustomAttribute(PropertyInfos[i], typeof(MapFromAttribute));

                if (attribute == null) continue; // no attribute on this property

                if (attribute.GetTargetFieldName == null)
                {
                    throw new ArgumentNullException($"No target field name specified for {PropertyInfos[i].Name}");
                }

                // store the list of mappable properties
                var originMember = MapsFrom.GetProperty(attribute.GetTargetFieldName);

                if (originMember == null)
                {
                    throw new ArgumentException($"Unable to find property with name {attribute.GetTargetFieldName} on type {MapsFrom.Name}");
                }

                if (AttributeMapping == null)
                {
                    AttributeMapping = new();
                }

                if (AttributeMapping.ContainsKey(originMember))
                {
                    throw new ArgumentException($"Unable to map {attribute.GetTargetFieldName} to multiple properties");
                }

                AttributeMapping.Add(originMember, PropertyInfos[i]);
            }
        }
    }
}

