#region DataTableExtensions 文件信息
/***********************************************************
**文 件 名：DataTableExtensions 
**命名空间：CommonLibrary.Extensions 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2016-10-22 21:26:39 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CommonLibrary.Extensions
{
    public static class DataTableExtensions
    {
        private static DataTable dt;
        /// <summary>
        /// DataTable转换为对象List集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> MapTo<T>(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return new List<T>();
            }

            return (from DataRow item in dt.Rows select MapTo<T>(item)).ToList();
        }

        /// <summary>
        /// DataRow转换为指定对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dr">DataRow</param>
        /// <returns></returns>
        public static T MapTo<T>(this DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }
            T model;
            var type = typeof(T);
            var modelType = GetModelType(type);
            switch (modelType)
            {
                //值类型
                case ModelType.Struct:
                    {
                        model = default(T);
                        if (dr[0] != null)
                            model = (T)dr[0];
                    }
                    break;
                //值类型
                case ModelType.Enum:
                    {
                        model = default(T);
                        if (dr[0] != null)
                        {
                            var fiType = dr[0].GetType();
                            if (fiType == typeof(int))
                            {
                                model = (T)dr[0];
                            }
                            else if (fiType == typeof(string))
                            {
                                model = (T)Enum.Parse(typeof(T), dr[0].ToString());
                            }
                        }
                    }
                    break;
                //引用类型 c#对string也当做值类型处理
                case ModelType.String:
                    {
                        model = default(T);
                        if (dr[0] != null)
                            model = (T)dr[0];
                    }
                    break;
                //引用类型 直接返回第一行第一列的值
                case ModelType.Object:
                    {
                        model = default(T);
                        if (dr[0] != null)
                            model = (T)dr[0];
                    }
                    break;

                //引用类型
                case ModelType.Else:
                    {
                        //引用类型 必须对泛型实例化
                        model = Activator.CreateInstance<T>();
                        //获取model中的属性
                        var modelPropertyInfos = type.GetProperties();
                        //遍历model每一个属性并赋值DataRow对应的列
                        foreach (var pi in modelPropertyInfos)
                        {
                            //获取属性名称
                            var name = pi.Name;
                            if (!dr.Table.Columns.Contains(name) || dr[name] == null) continue;
                            var piType = GetModelType(pi.PropertyType);
                            switch (piType)
                            {
                                case ModelType.Struct:
                                    {
                                        var value = Convert.ChangeType(dr[name], pi.PropertyType);
                                        pi.SetValue(model, value, null);
                                    }
                                    break;
                                case ModelType.Enum:
                                    {
                                        var fiType = dr[0].GetType();
                                        if (fiType == typeof(int))
                                        {
                                            pi.SetValue(model, dr[name], null);
                                        }
                                        else if (fiType == typeof(string))
                                        {
                                            var value = (T)Enum.Parse(typeof(T), dr[name].ToString());
                                            if (value != null)
                                                pi.SetValue(model, value, null);
                                        }
                                    }
                                    break;
                                case ModelType.String:
                                    {
                                        var value = Convert.ChangeType(dr[name], pi.PropertyType);
                                        pi.SetValue(model, value, null);
                                    }
                                    break;
                                case ModelType.Object:
                                    {
                                        pi.SetValue(model, dr[name], null);
                                    }
                                    break;
                                case ModelType.Else:
                                    throw new Exception("不支持该类型转换");
                                default:
                                    throw new Exception("未知类型");
                            }
                        }
                    }
                    break;
                default:
                    model = default(T);
                    break;
            }
            return model;
        }

        #region Computer

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>计算结果</returns>
        public static object Compute(this string expression)
        {
            return Compute(expression, "");
        }

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>计算结果</returns>
        public static T Compute<T>(this string expression)
        {
            return Compute<T>(expression, "");
        }

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="filter">过滤</param>
        /// <returns>计算结果</returns>
        public static object Compute(this string expression, string filter)
        {
            if (dt == null)
            {
                dt = new DataTable();
            }
            return new DataTable().Compute(expression, filter);
        }

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="filter">过滤</param>
        /// <returns>计算结果</returns>
        public static T Compute<T>(this string expression, string filter)
        {
            return (T)Compute(expression, filter);
        }

        #endregion


        #region 实体类转换成DataTable

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="models">实体类列表</param>
        /// <returns></returns>
        public static DataTable MapToDataTable<TModel>(this IEnumerable<TModel> models)
        {
            var enumerable = models as TModel[] ?? models.ToArray();
            if (models == null || !enumerable.Any())
            {
                return null;
            }
            var dt = CreateDataTableStructure(enumerable[0]);

            foreach (var model in enumerable)
            {
                var dataRow = dt.NewRow();
                foreach (var propertyInfo in typeof(TModel).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public static DataTable CreateDataTableStructure<TModel>(this TModel model)
        {
            if (model == null)
            {
                return null;
            }
            var dataTable = new DataTable(typeof(TModel).Name);
            foreach (var propertyInfo in typeof(TModel).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        #endregion

        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="modelType"></param>
        /// <returns></returns>
        private static ModelType GetModelType(Type modelType)
        {
            //值类型
            if (modelType.IsEnum)
            {
                return ModelType.Enum;
            }

            //值类型
            if (modelType.IsValueType)
            {
                return ModelType.Struct;
            }

            //引用类型 特殊类型处理
            if (modelType == typeof(string))
            {
                return ModelType.String;
            }

            //引用类型 特殊类型处理
            return modelType == typeof(object)
                ? ModelType.Object
                : ModelType.Else;
        }

        /// <summary>
        /// 类型枚举
        /// </summary>
        private enum ModelType
        {
            /// <summary>
            /// 值类型
            /// </summary>
            Struct,

            /// <summary>
            /// 枚举类型
            /// </summary>
            Enum,

            /// <summary>
            /// 引用类型
            /// </summary>
            String,

            /// <summary>
            /// 引用类型
            /// </summary>
            Object,

            /// <summary>
            /// 其他类型
            /// </summary>
            Else
        }
    }
}
