#region 文件信息和作者

/*************************************************************************************
      * CLR 版本:   4.0.30319.42000
      * 类 名 称:   DistinctExtensions
      * 机器名称:   LVJUNLEI-PC
      * 命名空间:   CommonLibrary.Extensions
      * 文 件 名:   DistinctExtensions
      * 创建时间:   16.5.5 16:46:38
      * 作   者:    LvJunlei

  * 创建年份:       2016
      * 修改时间:
      * 修 改 人:
*************************************************************************************
 * Copyright @ 凯亚开发部 2016. All rights reserved.
*************************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary.Extensions
{
    /// <summary>
    /// Distinct扩展集合
    /// </summary>
    public static class DistinctExtensions
    {
        /// <summary>
        /// Distinct扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector,
            IEqualityComparer<TV> comparer = default(EqualityComparer<TV>))
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector, comparer));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private readonly Func<T, TV> _keySelector;
        private readonly IEqualityComparer<TV> _comparer;

        public CommonEqualityComparer(Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            _keySelector = keySelector;
            _comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, TV> keySelector)
            : this(keySelector, EqualityComparer<TV>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}
