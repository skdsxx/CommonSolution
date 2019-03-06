﻿#region ExceptionExtensions 文件信息
/***********************************************************
**文 件 名：ExceptionExtensions 
**命名空间：CommonLibrary.Extensions 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2016-11-30 22:35:41 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Text;

namespace CommonLibrary.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 获取详细错误堆栈信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string DetailMessage(this Exception ex)
        {
            var expt = ex;
            var sb = new StringBuilder();
            while (expt != null)
            {
                if (!expt.Message.IsNullOrEmpty())
                {
                    sb.AppendLine("→" + expt.Message);
                }
                expt = expt.InnerException;
            }
            return sb.ToString();
        }
    }
}
