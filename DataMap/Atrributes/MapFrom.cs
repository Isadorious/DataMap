using System;
namespace DataMap.Atrributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public class MapFromAttribute : Attribute
	{
		private readonly Type? _targetType;
		private readonly string? _targetFieldName;

		public Type? GetTargetType
		{
			get { return this._targetType; }
		}

		public string? GetTargetFieldName
		{
			get { return this._targetFieldName; }
		}

		public MapFromAttribute(Type targetType)
		{
			_targetType = targetType;
		}

		public MapFromAttribute(string targetFieldName)
		{
			_targetFieldName = targetFieldName;
		}
	}
}

