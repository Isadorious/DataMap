using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using DataMap.Singleton;

namespace DataMap.Mapper
{
	internal class ExpressionMapper<TInput, TOutput> : ExpressionVisitor where TInput : class where TOutput : class 
	{
        internal Expression<Func<TOutput, bool>> Convert(Expression<Func<TInput, bool>> expression)
        {
            return (Expression<Func<TOutput, bool>>)Visit(expression);
        }

        private ParameterExpression replaceParam;

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (typeof(T) == typeof(Func<T, bool>))
            {
                replaceParam = Expression.Parameter(typeof(TOutput), "p");
                return Expression.Lambda<Func<T, bool>>(Visit(node.Body), replaceParam);
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
                var mapping = MappingDataSingleton.Instance.TypeMaps.Single(x => x.Key == typeof(TInput));

                MemberInfo member = mapping.Value.AttributeMapping.Where(x => x.Key.Name == node.Member.Name).Single().Value;

                if (member == null)
                    throw new InvalidOperationException("Cannot identify corresponding member of DataObject");
                return Expression.MakeMemberAccess(Visit(node.Expression), member);
            }
            return base.VisitMember(node);
        }
    }
}
