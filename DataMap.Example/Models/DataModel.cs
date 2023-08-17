using System;
namespace DataMap.Example.Models
{
	public class DataModel
	{
		public DataModel()
		{
		}

		public int DataModelId { get; set; }
		public string DataModelName { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool? IsEnabled { get; set; }

        public override string ToString()
        {
			return $"{DataModelId} {DataModelName} {CreatedAt} {IsEnabled}";
        }
    }
}

