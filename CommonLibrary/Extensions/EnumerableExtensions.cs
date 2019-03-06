#region IEnumerableExtensions 文件信息
/***********************************************************
**文 件 名：IEnumerableExtensions 
**命名空间：CommonLibrary.Extensions 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2016-10-14 10:59:15 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Iterator<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            var iteratorVariable = new Set<TSource>(comparer);
            foreach (var local in second)
            {
                iteratorVariable.Add(local);
            }
            foreach (var iteratorVariable1 in first)
            {
                if (!iteratorVariable.Add(iteratorVariable1))
                {
                    continue;
                }
                yield return iteratorVariable1;
            }
        }

        public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            var removingItemsDictionary = new Dictionary<T, int>();
            var _count = source.Count();
            var _items = new T[_count];
            var j = 0;
            foreach (var item in items)
            {
                if (!removingItemsDictionary.ContainsKey(item))
                {
                    removingItemsDictionary[item] = 1;
                }
            }
            for (var i = 0; i < _count; i++)
            {
                var current = source.ElementAt(i);
                if (!removingItemsDictionary.ContainsKey(current))
                {
                    _items[j++] = current;
                }
            }
            return _items;
        }
    }
}
