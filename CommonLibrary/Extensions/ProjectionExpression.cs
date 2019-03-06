#region ProjectionExpression 文件信息
/***********************************************************
**文 件 名：ProjectionExpression 
**命名空间：CommonLibrary.AutoMapper 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017/9/15 10:57:38 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CommonLibrary.Extensions
{
    public class ProjectionExpression<TSource>
    {
        private static readonly Dictionary<string, Expression> ExpressionCache = new Dictionary<string, Expression>();

        private readonly IQueryable<TSource> _source;

        public ProjectionExpression(IQueryable<TSource> source)
        {
            _source = source;
        }

        public IQueryable<TDest> To<TDest>()
        {
            var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();

            return _source.Select(queryExpression);
        }

        private static Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            var key = GetCacheKey<TDest>();

            return ExpressionCache.ContainsKey(key) ? ExpressionCache[key] as Expression<Func<TSource, TDest>> : null;
        }

        private static Expression<Func<TSource, TDest>> BuildExpression<TDest>()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(TSource), "src");

            var bindings = destinationProperties
                                .Select(destinationProperty => BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                                .Where(binding => binding != null);

            var expression = Expression.Lambda<Func<TSource, TDest>>(Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

            var key = GetCacheKey<TDest>();

            ExpressionCache.Add(key, expression);

            return expression;
        }

        private static MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty, IEnumerable<PropertyInfo> sourceProperties)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty != null)
            {
                return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
            }

            var propertyNames = SplitCamelCase(destinationProperty.Name);

            if (propertyNames.Length == 2)
            {
                sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == propertyNames[0]);

                if (sourceProperty != null)
                {
                    var sourceChildProperty = sourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[1]);

                    if (sourceChildProperty != null)
                    {
                        return Expression.Bind(destinationProperty, Expression.Property(Expression.Property(parameterExpression, sourceProperty), sourceChildProperty));
                    }
                }
            }

            return null;
        }

        private static string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }

        private static string[] SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' ');
        }
    }
}
