#region 文件信息和作者

/*************************************************************************************
      * CLR 版本:   4.0.30319.42000
      * 类 名 称:   ExpressionExtensions
      * 机器名称:   LVJUNLEI-PC
      * 命名空间:   CommonLibrary.Extensions
      * 文 件 名:   ExpressionExtensions
      * 创建时间:   16.5.5 16:55:35
      * 作   者:    LvJunlei

  * 创建年份:       2016
      * 修改时间:
      * 修 改 人:
*************************************************************************************
 * Copyright @ 凯亚开发部 2016. All rights reserved.
*************************************************************************************/
#endregion

using System.Linq.Expressions;

namespace CommonLibrary.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }
        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }
        public static Expression<TDelegate> ToLambda<TDelegate>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<TDelegate>(body, parameters);
        }
    }
}
