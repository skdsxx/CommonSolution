#region QueryableExtensions 文件信息
/***********************************************************
**文 件 名：QueryableExtensions 
**命名空间：CommonLibrary.Extensions 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017/9/15 11:02:36 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System.Linq;

namespace CommonLibrary.Extensions
{
    public static class QueryableExtensions
    {
        public static ProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
    }
}
