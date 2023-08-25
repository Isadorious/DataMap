// See https://aka.ms/new-console-template for more information
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using DataMap.Example.Models;
using DataMap.Mapper;

Console.WriteLine("Hello, World!");

DataModel dataModel = new()
{
    DataModelId = 1,
    DataModelName = "Test Model",
    IsEnabled = true,
    CreatedAt = DateTime.Now,
};

Console.WriteLine($"Created data model:: {dataModel}");

//logicModel.Map(dataModel);
var logicModel = Mapper.Map<DataModel, LogicModel>(dataModel);

Console.WriteLine("Mapped logic model");
Console.WriteLine($"logic model:: {logicModel}");

LogicModel logicModel2 = new();
logicModel2.Id = 2;
logicModel2.IsEnabled = true;
logicModel2.Name = "Reverse map test";
logicModel2.CreatedAt = DateTime.Now;

Console.WriteLine("Created logic model");
Console.WriteLine($"logicModel2:: {logicModel2}");

//var dataModel2 = logicModel2.ReverseMap<DataModel>();

var dataModel2 = Mapper.ReverseMap<LogicModel, DataModel>(logicModel2);
Expression<Func<LogicModel, bool>> logicExpr = x => x.Id == 1;
Expression<Func<DataModel, bool>> dataExpr = (Expression<Func<DataModel, bool>>)DataMap.Mapper.Mapper.MapExpresion<LogicModel, DataModel>(logicExpr);

Console.WriteLine("Reverse mapped data model");
Console.WriteLine($"dataModel2:: {dataModel2}");