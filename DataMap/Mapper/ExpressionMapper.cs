using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using DataMap.Singleton;

namespace DataMap.Mapper
{
	internal class ExpressionMapper<TInput, TOutput> : ExpressionVisitor
	{
        internal Expression<Func<TOutput, bool>> Convert(Expression<Func<TInput, bool>> expression)
        {
            return (Expression<Func<TOutput, bool>>)Visit(expression);
        }

        private ParameterExpression replaceParam;

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (typeof(T) == typeof(Func<TInput, bool>))
            {
                replaceParam = Expression.Parameter(typeof(TOutput), "p");
                return Expression.Lambda<Func<TOutput, bool>>(Visit(node.Body), replaceParam);
            }
            return base.VisitLambda<T>(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(TInput))
                return replaceParam; // Expression.Parameter(typeof(DataObject), "p");
            return base.VisitParameter(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TInput))
            {
                var mapping = MappingDataSingleton.Instance.TypeMaps.SingleOrDefault(x => x.Key == typeof(TInput));

                PropertyInfo? property = mapping.Value.AttributeMapping.Where(x => x.Key.Name == node.Member.Name).FirstOrDefault().Value;
                if (property == null)
                    property = mapping.Value.AttributeMapping.Where(x => x.Value.Name == node.Member.Name).FirstOrDefault().Key;

                if (property == null)
                    throw new InvalidOperationException($"Cannot identify corresponding member of DataObject for");

                MemberInfo member = typeof(TOutput).GetMember(property.Name, BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();

                if (member == null) throw new InvalidOperationException("Member is null");
                return Expression.MakeMemberAccess(Visit(node.Expression), member);
            }
            return base.VisitMember(node);
        }
    }
}
