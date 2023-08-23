using System;
using DataMap.Mapper;
using DataMap.Atrributes;
using Xunit;
using System.Linq.Expressions;
using System.Linq;
namespace DataMap.Tests.Mapper
{
	[MapFrom(typeof(DataObject))]
    [MapReversible]
	public class LogicObject
	{
        [MapFrom("DataId")]
        public int Id { get; set; }
        [MapFrom("DataName")]
        public string Name { get; set; }
        [MapFrom("DataCreatedAt")]
        public DateTime CreatedAt { get; set; }
        [MapFrom("DataIsEnabled")]
        public bool IsEnabled { get; set; }
    }

	public class DataObject
	{
        public int DataId { get; set; }
        public string DataName { get; set; }
        public DateTime DataCreatedAt { get; set; }
        public bool DataIsEnabled { get; set; }
    }

	public class MapperTests
	{
        [Fact]
        public void Test_Map()
        {
            DataObject data = new()
            {
                DataId = 1,
                DataName = "Test",
                DataCreatedAt = new DateTime(2023, 8, 23),
                DataIsEnabled = true,
            };

            LogicObject? mappedData = DataMap.Mapper.Mapper.Map<DataObject, LogicObject>(data);

            Assert.True(mappedData != null);
            Assert.True(mappedData.Id == 1);
            Assert.True(mappedData.Name == "Test");
            Assert.True(mappedData.IsEnabled == true);
            Assert.True(mappedData.CreatedAt == new DateTime(2023, 8, 23));
        }

        [Fact]
        public void Test_ReverseMap()
        {
            LogicObject mappedData = new()
            {
                Id = 1,
                Name = "Test",
                CreatedAt = new DateTime(2023, 8, 23),
                IsEnabled = true,
            };

            DataObject? data = DataMap.Mapper.Mapper.ReverseMap<LogicObject, DataObject>(mappedData);

            Assert.True(data != null);
            Assert.True(data.DataId == 1);
            Assert.True(data.DataName == "Test");
            Assert.True(data.DataIsEnabled == true);
            Assert.True(data.DataCreatedAt == new DateTime(2023, 8, 23));
        }

        [Fact]
        public void Test_ExpressionMap()
        {
            DataObject[] dataObjects = { new DataObject() { DataId = 1, DataName = "Test", DataCreatedAt = new DateTime(2023, 8, 23) }, new DataObject() { DataId = 2, DataName = "Test 2", DataCreatedAt = new DateTime(2023, 8, 23) } };
            LogicObject[] logicObjects = { new LogicObject() { Id = 1, Name = "Test", CreatedAt = new DateTime(2023, 8, 23) }, new LogicObject() { Id = 1, Name = "Test 2", CreatedAt = new DateTime(2023, 8, 23) } };


            Expression<Func<LogicObject, bool>> logicExpr = x => x.Id == 1;
            Expression<Func<DataObject, bool>> dataExpr = (Expression<Func<DataObject, bool>>) DataMap.Mapper.Mapper.MapExpresion<LogicObject, DataObject>(logicExpr);

            var dataObject = dataObjects.Where(dataExpr.Compile()).FirstOrDefault();
            var logicObject = logicObjects.Where(logicExpr.Compile()).FirstOrDefault();

            Assert.True(dataObject != null);
            Assert.True(logicObject != null);
            Assert.True(dataObject.DataId == logicObject.Id);
        }
	}
}

