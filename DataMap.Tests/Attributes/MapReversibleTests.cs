using System;
using DataMap.Atrributes;
using System.Collections.Generic;
using Xunit;

namespace DataMap.Tests.Attributes
{
	public class MapReversibleTests
	{
        [Fact]
        public void MapFrom_DisallowMultiple()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(MapReversibleAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.True(attributes.Count == 1);

            var attribute = attributes[0];
            Assert.False(attribute.AllowMultiple);
        }

        [Fact]
        public void MapFrom_TargetsClass()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(MapReversibleAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.True(attributes.Count == 1);

            var attribute = attributes[0];
            Assert.True(attribute.ValidOn.HasFlag(AttributeTargets.Class));
        }

        [Fact]
        public void MapsFrom_NoInherit()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(MapReversibleAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.True(attributes.Count == 1);

            var attribute = attributes[0];
            Assert.False(attribute.Inherited);
        }
    }
}

