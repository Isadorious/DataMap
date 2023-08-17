// See https://aka.ms/new-console-template for more information
using DataMap.Example.Models;

Console.WriteLine("Hello, World!");

DataModel dataModel = new()
{
    DataModelId = 1,
    DataModelName = "Test Model",
    IsEnabled = true,
    CreatedAt = DateTime.Now
};

Console.WriteLine($"Created data model:: {dataModel}");

LogicModel logicModel = new();

logicModel.Map(dataModel);

Console.WriteLine("Mapped logic model");
Console.WriteLine($"logic model:: {logicModel}");

LogicModel logicModel2 = new();
logicModel2.Id = 2;
logicModel2.IsEnabled = true;
logicModel2.Name = "Reverse map test";
logicModel2.CreatedAt = DateTime.Now;

Console.WriteLine("Created logic model");
Console.WriteLine($"logicModel2:: {logicModel2}");

var dataModel2 = logicModel2.ReverseMap<DataModel>();

Console.WriteLine("Reverse mapped data model");
Console.WriteLine($"dataModel2:: {dataModel2}");