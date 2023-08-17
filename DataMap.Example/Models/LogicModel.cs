using System;
using DataMap.Atrributes;
using DataMap.Mapper;
namespace DataMap.Example.Models
{
    [MapFrom(typeof(DataModel))]
    [MapReversible]
	public class LogicModel : Mappable<LogicModel>
	{
		public LogicModel()
		{
		}

        [MapFrom("DataModelId")]
        public int Id { get; set; }
        [MapFrom("DataModelName")]
        public string Name { get; set; }
        [MapFrom("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [MapFrom("IsEnabled")]
        public bool IsEnabled { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} {CreatedAt} {IsEnabled}";
        }
    }
}

