#region IntPtrExtensions 文件信息
/***********************************************************
**文 件 名：IntPtrExtensions 
**命名空间：CommonLibrary.Extensions 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017/12/24 9:19:23 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Runtime.InteropServices;

namespace CommonLibrary.Extensions
{
    /// <summary>
    /// IntPtr扩展方法
    /// </summary>
    public static class IntPtrExtensions
    {
        /// <summary>
        /// 删除指针指向的对象
        /// </summary>
        /// <param name="hObject">指针</param>
        /// <returns></returns>
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 删除指针指向的对象
        /// </summary>
        /// <param name="hObject">指针</param>
        /// <returns></returns>
        public static bool Delete(this IntPtr hObject)
        {
            return DeleteObject(hObject);
        }
    }
}
