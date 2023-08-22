using System;
using System.Reflection;
using System.Xml.Serialization;
using DataMap.Atrributes;
using DataMap.Singleton;
using DataMap.Models;
using System.Linq.Expressions;

namespace DataMap.Mapper
{
	public class Mappable<T> where T : class
	{

		public T? Map<U>(U mapFrom) where U : class
		{
			var res = Mapper.Map<U, T>(mapFrom);
			return res;
		}

        public U? ReverseMap<U>() where U : class
		{
			var res = Mapper.Map<T, U>(this as T);
			return res;
        }

		public Expression MapExpresion<U>(Expression<Func<T, bool>> expression) where U : class
		{
			var res = Mapper.MapExpresion<T, U>(expression);
			return res;
		}
    }
}

