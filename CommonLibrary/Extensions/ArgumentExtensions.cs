#region 文件和作者信息
/******************************************************************

  * CLR 版本: 4.0.30319.42000
  * 机器名称: SUNXIAN
  * 命名空间: CommonLibrary.Extensions
  * 文 件 名: ArgumentExtensions
  * 创建人员: SunXian
  * 创建时间: 2017/4/26 10:37:54
  
//******************************************************************
* Copyright： @青岛凯亚研发部@
******************************************************************/
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CommonLibrary.Helpers;

namespace CommonLibrary.Extensions
{
    public static  class ArgumentExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="argName"></param>
        public static void AssertEnumMember<TEnum>(this TEnum enumValue, string argName) where TEnum : struct, IConvertible
        {
            ArgumentHelper.AssertEnumMember(enumValue, argName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="argName"></param>
        /// <param name="validValues"></param>
        public static void AssertEnumMember<TEnum>(this TEnum enumValue, string argName, params TEnum[] validValues) where TEnum : struct, IConvertible
        {
            ArgumentHelper.AssertEnumMember(enumValue, argName, validValues);
        }

        [DebuggerHidden]
        public static void AssertGenericArgumentNotNull<T>(this T arg, string argName)
        {
            ArgumentHelper.AssertGenericArgumentNotNull(arg, argName);
        }

        [DebuggerHidden]
        public static void AssertNotNull<T>(this T arg, string argName) where T : class
        {
            ArgumentHelper.AssertNotNull(arg, argName);
        }

        [DebuggerHidden]
        public static void AssertNotNull<T>(this T? arg, string argName) where T : struct
        {
            ArgumentHelper.AssertNotNull(arg, argName);
        }

        [DebuggerHidden]
        public static void AssertNotNull<T>(this IEnumerable<T> arg, string argName, bool assertContentsNotNull)
        {
            ArgumentHelper.AssertNotNull(arg, argName, assertContentsNotNull);
        }

        [DebuggerHidden]
        public static void AssertNotNullOrEmpty(this ICollection arg, string argName)
        {
            ArgumentHelper.AssertNotNullOrEmpty(arg, argName);
        }

        [DebuggerHidden]
        public static void AssertNotNullOrEmpty(this string arg, string argName)
        {
            ArgumentHelper.AssertNotNullOrEmpty(arg, argName);
        }

        [DebuggerHidden]
        public static void AssertNotNullOrEmpty(this string arg, string argName, bool trim)
        {
            ArgumentHelper.AssertNotNullOrEmpty(arg, argName, trim);
        }
    }
}
