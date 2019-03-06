#region 文件和作者信息
/******************************************************************

  * CLR 版本: 4.0.30319.42000
  * 机器名称: DESKTOP-L71O6PR
  * 命名空间: CommonLibrary.AutoMapper
  * 文 件 名: AutoMapperExtension
  * 创建人员: SunXian
  * 创建时间: 2018/9/14 15:57:16

//******************************************************************
* Copyright： @青岛凯亚研发部@
******************************************************************/
#endregion
using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonLibrary.AutoMapper
{
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 对象到对象
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>

        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            Mapper.Initialize(ctx => ctx.CreateMap(obj.GetType(), typeof(T)));
            return Mapper.Map<T>(obj);
        }

        /// <summary>
        /// 集合到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> MapTo<T>(this IEnumerable obj)
        {
            if (obj == null) throw new ArgumentNullException();
            Mapper.Initialize(ctx => ctx.CreateMap(obj.GetType(), typeof(T)));
            return Mapper.Map<List<T>>(obj);

        }
    }
}
