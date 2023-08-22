using System;
using System.Linq.Expressions;
using System.Reflection;
using DataMap.Models;
using DataMap.Singleton;

namespace DataMap.Mapper
{
	public class Mapper
	{
        private static MappingData GetMappingData<TOutput>()
        {
            if (!MappingDataSingleton.Instance.TypeMaps.ContainsKey(typeof(TOutput)))
            {
                MappingDataSingleton.Instance.TypeMaps.Add(typeof(TOutput), new MappingData(typeof(TOutput)));
            }

            return MappingDataSingleton.Instance.TypeMaps.Single(x => x.Key == typeof(TOutput)).Value;
        }

        /// <summary>
        /// Maps the provided object using the mapping data setup via class attributes e.g. DataObject -> LogicObject
        /// </summary>
        /// <typeparam name="TInput">The type the data is being mapped from</typeparam>
        /// <typeparam name="TOutput">The type that is being mapped to</typeparam>
        /// <param name="objectToMap">The object that contains the data to be mapped</param>
        /// <returns>The mapped object</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static TOutput? Map<TInput, TOutput>(TInput objectToMap) where TInput : class where TOutput : class
		{
            var mappingData = GetMappingData<TOutput>();

            if (typeof(TInput) != mappingData.MapsFrom)
            {
                throw new ArgumentException($"Cannot map between {typeof(TInput)} and {typeof(TOutput)} please use a {mappingData.MapsFrom?.Name} type instead");
            }

            if (objectToMap == null)
            {
                throw new NullReferenceException($"Cannot map a null object");
            }

            if (mappingData.MapsTo == null) throw new NullReferenceException("Cannot map to null");
            if (mappingData.AttributeMapping == null) throw new NullReferenceException("No data mapping found");
            // using the information contained within attributeMapping convert U to T
            // iterate over the properties in T then set the properties from the content in U if a mapping exists
            // no mapping setup = no copy
            object toRet = Activator.CreateInstance(mappingData.MapsTo) ?? throw new NullReferenceException("Cannot map to null");

            PropertyInfo[] propertyList = mappingData.MapsTo.GetProperties();

            foreach (var targetProperty in propertyList)
            {
                if (targetProperty == null) continue;

                // get origin property from attribute mapping dictionary
                var originProperty = mappingData.AttributeMapping.FirstOrDefault(x => x.Value.Name == targetProperty.Name && x.Value.MemberType == targetProperty.MemberType).Key;

                if (originProperty == null)
                {
                    throw new ArgumentException($"Unable to find origin property for {targetProperty.Name}");
                }

                var originValue = originProperty.GetValue(objectToMap);

                targetProperty.SetValue(toRet, originValue);
            }

            return toRet as TOutput;
        }

        /// <summary>
        /// Performs a reverse mapping of the object using the mapping data setup via class attributes. e.g. LogicObject -> DataObject
        /// </summary>
        /// <typeparam name="TInput">The type the data is being mapped from</typeparam>
        /// <typeparam name="TOutput">The type the data is being mapped to</typeparam>
        /// <param name="objectToMap">The object that contains the data to be mapped</param>
        /// <returns>The reverse mapped object</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static TOutput? ReverseMap<TInput, TOutput>(TInput objectToMap) where TInput : class where TOutput : class
        {
            var mappingData = GetMappingData<TInput>();

            if (mappingData.IsRevserable != true)
            {
                throw new ArgumentException($"Reverse mapping not enabled for type {typeof(TInput)}");
            }

            if (typeof(TOutput) != mappingData.MapsFrom)
            {
                throw new ArgumentException($"Cannot map between {typeof(TInput)} and {typeof(TOutput)} please use a {mappingData.MapsFrom?.Name} type instead");
            }

            if (mappingData.AttributeMapping == null) throw new NullReferenceException("No data mapping found");

            // using the information contained within attributeMapping convert TInput to TOutput
            // iterate over the properties in U then set the properties from the content in T if a mapping exists
            // no mapping setup = no copy
            object toRet = Activator.CreateInstance(mappingData.MapsFrom) ?? throw new NullReferenceException("Cannot map to null");
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

                var originValue = originProperty.GetValue(objectToMap);
                targetProperty.SetValue(toRet, originValue);

            }

            return toRet as TOutput;
        }

        /// <summary>
        /// Maps the given LINQ expression using the mapping data specified via class attributes. Will map both ways.
        /// </summary>
        /// <typeparam name="TInput">The type of the object you are converting the expression from</typeparam>
        /// <typeparam name="TOutput">The type of the object you are converting the expression to</typeparam>
        /// <param name="expression">The expression to be converted</param>
        /// <returns>The mapped expression</returns>
        public static Expression MapExpresion<TInput, TOutput>(Expression<Func<TInput, bool>> expression) where TInput : class where TOutput : class
        {
            ExpressionMapper<TInput, TOutput> expressionMapper = new();

            return expressionMapper.Convert(expression);
        }
    }
}

