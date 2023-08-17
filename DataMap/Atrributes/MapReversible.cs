using System;
namespace DataMap.Atrributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class MapReversibleAttribute : Attribute
	{
		public MapReversibleAttribute()
		{
		}
	}
}

