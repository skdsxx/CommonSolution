#region 文件和作者信息
/******************************************************************

  * CLR 版本: 4.0.30319.42000
  * 机器名称: SUNXIAN
  * 命名空间: CommonLibrary.ListExtention
  * 文 件 名: DistinctByListExtention
  * 创建人员: SunXian
  * 创建时间: 2017/4/26 10:37:54
  
//******************************************************************
* Copyright： @青岛凯亚研发部@
******************************************************************/
#endregion

using System;
using System.Collections.Generic;

namespace CommonLibrary.ListExtention
{
    public static class DistinctByListExtention
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
