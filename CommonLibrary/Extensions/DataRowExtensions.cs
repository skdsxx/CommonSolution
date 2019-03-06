#region 文件和作者信息
/******************************************************************

  * CLR 版本: 4.0.30319.42000
  * 机器名称: SUNXIAN
  * 命名空间: CommonLibrary.Extensions
  * 文 件 名: DataRowExtensions
  * 创建人员: SunXian
  * 创建时间: 2017/4/26 10:37:54
  
//******************************************************************
* Copyright： @青岛凯亚研发部@
******************************************************************/
#endregion

using System;
using System.Data;

namespace CommonLibrary.Extensions
{
    public static class DataRowExtensions
    {
        /// <summary>
        ///  This method provides access to the values in each of the columns in a given row. 
        ///  This method makes casts unnecessary when accessing columns. 
        ///  Additionally, Field supports nullable types and maps automatically between DBNull and 
        ///  Nullable when the generic type is nullable. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow
        /// </param>
        /// <param name="columnName">
        ///   The input column name specificy which row value to retrieve.
        /// </param>
        /// <returns>
        ///   The DataRow value for the column specified.
        /// </returns> 
        public static T GetField<T>(this DataRow row, string columnName)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            if (columnName.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(columnName));
            }
            return UnboxT<T>.Unbox(row[columnName]);
        }

        /// <summary>
        ///  This method provides access to the values in each of the columns in a given row. 
        ///  This method makes casts unnecessary when accessing columns. 
        ///  Additionally, Field supports nullable types and maps automatically between DBNull and 
        ///  Nullable when the generic type is nullable. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow
        /// </param>
        /// <param name="column">
        ///   The input DataColumn specificy which row value to retrieve.
        /// </param>
        /// <returns>
        ///   The DataRow value for the column specified.
        /// </returns> 
        public static T GetField<T>(this DataRow row, DataColumn column)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            if (column==null)
            {
                throw new ArgumentNullException(nameof(column));
            }
            return UnboxT<T>.Unbox(row[column]);
        }

        /// <summary>
        ///  This method provides access to the values in each of the columns in a given row. 
        ///  This method makes casts unnecessary when accessing columns. 
        ///  Additionally, Field supports nullable types and maps automatically between DBNull and 
        ///  Nullable when the generic type is nullable. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow
        /// </param>
        /// <param name="columnIndex">
        ///   The input ordinal specificy which row value to retrieve.
        /// </param>
        /// <returns>
        ///   The DataRow value for the column specified.
        /// </returns> 
        public static T GetField<T>(this DataRow row, int columnIndex)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            return UnboxT<T>.Unbox(row[columnIndex]);
        }

        /// <summary>
        ///  This method provides access to the values in each of the columns in a given row. 
        ///  This method makes casts unnecessary when accessing columns. 
        ///  Additionally, Field supports nullable types and maps automatically between DBNull and 
        ///  Nullable when the generic type is nullable. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow
        /// </param>
        /// <param name="columnIndex">
        ///   The input ordinal specificy which row value to retrieve.
        /// </param>
        /// <param name="version">
        ///   The DataRow version for which row value to retrieve.
        /// </param>
        /// <returns>
        ///   The DataRow value for the column specified.
        /// </returns> 
        public static T GetField<T>(this DataRow row, int columnIndex, DataRowVersion version)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            return UnboxT<T>.Unbox(row[columnIndex, version]);
        }

        /// <summary>
        ///  This method provides access to the values in each of the columns in a given row. 
        ///  This method makes casts unnecessary when accessing columns. 
        ///  Additionally, Field supports nullable types and maps automatically between DBNull and 
        ///  Nullable when the generic type is nullable. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow
        /// </param>
        /// <param name="columnName">
        ///   The input column name specificy which row value to retrieve.
        /// </param>
        /// <param name="version">
        ///   The DataRow version for which row value to retrieve.
        /// </param>
        /// <returns>
        ///   The DataRow value for the column specified.
        /// </returns> 
        public static T GetField<T>(this DataRow row, string columnName, DataRowVersion version)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            if (columnName.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(columnName));
            }
            return UnboxT<T>.Unbox(row[columnName, version]);
        }

        /// <summary>
        ///  This method provides access to the values in each of the columns in a given row. 
        ///  This method makes casts unnecessary when accessing columns. 
        ///  Additionally, Field supports nullable types and maps automatically between DBNull and 
        ///  Nullable when the generic type is nullable. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow
        /// </param>
        /// <param name="column">
        ///   The input DataColumn specificy which row value to retrieve.
        /// </param>
        /// <param name="version">
        ///   The DataRow version for which row value to retrieve.
        /// </param>
        /// <returns>
        ///   The DataRow value for the column specified.
        /// </returns> 
        public static T GetField<T>(this DataRow row, DataColumn column, DataRowVersion version)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            if (column==null)
            {
                throw new ArgumentNullException(nameof(column));
            }
            return UnboxT<T>.Unbox(row[column, version]);
        }

        /// <summary>
        ///  This method sets a new value for the specified column for the DataRow it’s called on. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow.
        /// </param>
        /// <param name="columnIndex">
        ///   The input ordinal specifying which row value to set.
        /// </param>
        /// <param name="value">
        ///   The new row value for the specified column.
        /// </param>
        public static void SetField<T>(this DataRow row, int columnIndex, T value)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            row[columnIndex] = (object)value ?? DBNull.Value;
        }

        /// <summary>
        ///  This method sets a new value for the specified column for the DataRow it’s called on. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow.
        /// </param>
        /// <param name="columnName">
        ///   The input column name specificy which row value to retrieve.
        /// </param>
        /// <param name="value">
        ///   The new row value for the specified column.
        /// </param>
        public static void SetField<T>(this DataRow row, string columnName, T value)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            if (columnName.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(columnName));
            }
            row[columnName] = (object)value ?? DBNull.Value;
        }

        /// <summary>
        ///  This method sets a new value for the specified column for the DataRow it’s called on. 
        /// </summary>
        /// <param name="row">
        ///   The input DataRow.
        /// </param>
        /// <param name="column">
        ///   The input DataColumn specificy which row value to retrieve.
        /// </param>
        /// <param name="value">
        ///   The new row value for the specified column.
        /// </param>
        public static void SetField<T>(this DataRow row, DataColumn column, T value)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            if (column==null)
            {
                throw new ArgumentNullException(nameof(column));
            }
            row[column] = (object)value ?? DBNull.Value;
        }

        private static class UnboxT<T>
        {
            internal static readonly Converter<object, T> Unbox = Create(typeof(T));

            private static Converter<object, T> Create(Type type)
            {
                if (type.IsValueType)
                {
                    if (type.IsGenericType && !type.IsGenericTypeDefinition && (typeof(Nullable<>) == type.GetGenericTypeDefinition()))
                    {
                        return (Converter<object, T>)Delegate.CreateDelegate(
                            typeof(Converter<object, T>),
                                typeof(UnboxT<T>)
                                    .GetMethod("NullableField", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                                    .MakeGenericMethod(type.GetGenericArguments()[0]));
                    }
                    return ValueField;
                }
                return ReferenceField;
            }

            private static T ReferenceField(object value)
            {
                return ((DBNull.Value == value) ? default(T) : (T)value);
            }

            private static T ValueField(object value)
            {
                if (DBNull.Value == value)
                {
                    throw new InvalidCastException("value值不能为空");
                }
                return (T)value;
            }
        }
    }
}
