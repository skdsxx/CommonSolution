#region ArgumentHelper 文件信息
/***********************************************************
**文 件 名：ArgumentHelper 
**命名空间：CommonLibrary.Helpers 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017-02-24 13:44:57 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace CommonLibrary.Helpers
{
    public class ArgumentHelper
    {
        /// <summary>
        /// 插入枚举成员
        /// </summary>
        /// <typeparam name="TEnum">枚举类</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <param name="argName">参数名称</param>
        public static void AssertEnumMember<TEnum>(TEnum enumValue, string argName) where TEnum : struct, IConvertible
        {
            if (Attribute.IsDefined(typeof(TEnum), typeof(FlagsAttribute), false))
            {
                bool flag;
                var num = enumValue.ToInt64(CultureInfo.InvariantCulture);
                if (num == 0L)
                {
                    flag = !Enum.IsDefined(typeof(TEnum), ((IConvertible)0).ToType(Enum.GetUnderlyingType(typeof(TEnum)), CultureInfo.InvariantCulture));
                }
                else
                {
                    foreach (TEnum local in Enum.GetValues(typeof(TEnum)))
                    {
                        num &= ~local.ToInt64(CultureInfo.InvariantCulture);
                    }
                    flag = num != 0L;
                }
                if (flag)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Enum value '{0}' is not valid for flags enumeration '{1}'.", enumValue, typeof(TEnum).FullName), argName);
                }
            }
            else if (!Enum.IsDefined(typeof(TEnum), enumValue))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Enum value '{0}' is not defined for enumeration '{1}'.", enumValue, typeof(TEnum).FullName), argName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="argName"></param>
        /// <param name="validValues"></param>
        public static void AssertEnumMember<TEnum>(TEnum enumValue, string argName, params TEnum[] validValues) where TEnum : struct, IConvertible
        {
            bool flag;
            AssertNotNull(validValues, "validValues");
            if (!Attribute.IsDefined(typeof(TEnum), typeof(FlagsAttribute), false))
            {
                foreach (var local3 in validValues)
                {
                    if (enumValue.Equals(local3))
                    {
                        return;
                    }
                }
                if (!Enum.IsDefined(typeof(TEnum), enumValue))
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Enum value '{0}' is not defined for enumeration '{1}'.", enumValue, typeof(TEnum).FullName), argName);
                }
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Enum value '{0}' is defined for enumeration '{1}' but it is not permitted in this context.", enumValue, typeof(TEnum).FullName), argName);
            }
            var num = enumValue.ToInt64(CultureInfo.InvariantCulture);
            if (num == 0L)
            {
                flag = true;
                foreach (var local in validValues)
                {
                    if (local.ToInt64(CultureInfo.InvariantCulture) == 0L)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (var local2 in validValues)
                {
                    num &= ~local2.ToInt64(CultureInfo.InvariantCulture);
                }
                flag = num != 0L;
            }
            if (flag)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Enum value '{0}' is not allowed for flags enumeration '{1}'.", enumValue, typeof(TEnum).FullName), argName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        public static void AssertGenericArgumentNotNull<T>(T arg, string argName)
        {
            var type = typeof(T);
            if (!type.IsValueType || (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>))))
            {
                AssertNotNull<object>(arg, argName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        public static void AssertNotNull<T>(T arg, string argName) where T : class
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        public static void AssertNotNull<T>(T? arg, string argName) where T : struct
        {
            if (!arg.HasValue)
            {
                throw new ArgumentNullException(argName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        /// <param name="assertContentsNotNull"></param>
        public static void AssertNotNull<T>(IEnumerable<T> arg, string argName, bool assertContentsNotNull)
        {
            AssertNotNull(arg, argName);
            if (assertContentsNotNull && typeof(T).IsClass)
            {
                foreach (var local in arg)
                {
                    if (local == null)
                    {
                        throw new ArgumentException("An item inside the enumeration was null.", argName);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        public static void AssertNotNullOrEmpty(ICollection arg, string argName)
        {
            if ((arg == null) || (arg.Count == 0))
            {
                throw new ArgumentException("Cannot be null or empty.", argName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        public static void AssertNotNullOrEmpty(string arg, string argName)
        {
            AssertNotNullOrEmpty(arg, argName, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        /// <param name="trim"></param>
        public static void AssertNotNullOrEmpty(string arg, string argName, bool trim)
        {
            if (string.IsNullOrEmpty(arg) || (trim && IsOnlyWhitespace(arg)))
            {
                throw new ArgumentException("Cannot be null or empty.", argName);
            }
        }

        private static bool IsOnlyWhitespace(string arg)
        {
            foreach (var ch in arg)
            {
                if (!char.IsWhiteSpace(ch))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
