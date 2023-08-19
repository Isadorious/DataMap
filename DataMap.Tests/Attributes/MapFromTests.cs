using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Xunit;
using DataMap.Atrributes;

namespace DataMap.Tests.Attributes
{
	public class MapFromTests
	{
		[Fact]
        public void MapFrom_DisallowMultiple()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(MapFromAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.True(attributes.Count == 1);

            var attribute = attributes[0];
            Assert.False(attribute.AllowMultiple);
        }

        [Fact]
        public void MapFrom_TargetsClass()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(MapFromAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.True(attributes.Count == 1);

            var attribute = attributes[0];
            Assert.True(attribute.ValidOn.HasFlag(AttributeTargets.Class));
        }

        [Fact]
        public void MapFrom_TargetsProperty()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(MapFromAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.True(attributes.Count == 1);

            var attribute = attributes[0];
            Assert.True(attribute.ValidOn.HasFlag(AttributeTargets.Property));
        }

        [Fact]
        public void MapsFrom_NoInherit()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(MapFromAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.True(attributes.Count == 1);

            var attribute = attributes[0];
            Assert.False(attribute.Inherited);
        }

    }
}

