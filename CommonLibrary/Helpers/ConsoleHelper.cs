#region ConsoleHelper 文件信息
/***********************************************************
**文 件 名：ConsoleHelper 
**命名空间：CommonLibrary.Helpers 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2018/3/22 10:37:51 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CommonLibrary.Helpers
{
    /// <summary>
    /// ConsoleHelper
    /// </summary>
    public class ConsoleHelper
    {
        #region API

        private const string Kernel32DllName = "kernel32.dll";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(Kernel32DllName)]
        public static extern bool AllocConsole();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(Kernel32DllName)]
        public static extern bool FreeConsole();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(Kernel32DllName)]
        public static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(Kernel32DllName)]
        public static extern IntPtr GetConsoleOutputCP();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public static bool HasConsole => GetConsoleWindow() != IntPtr.Zero;

        /// <summary>
        /// 
        /// </summary>
        public static void Show()
        {
            if (!HasConsole)
            {
                AllocConsole();
                InvalidateOutAndError();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Close()
        {
            if (HasConsole)
            {
                SetOutAndErroNull();
                FreeConsole();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Toggle()
        {
            if (HasConsole)
            {
                Close();
            }
            else
            {
                Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InvalidateOutAndError()
        {
            var type = typeof(Console);

            var _out = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Static);

            var error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Static);

            var initializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Static);

            Debug.Assert(_out != null);
            Debug.Assert(error != null);
            Debug.Assert(initializeStdOutError != null);

            _out?.SetValue(null, null);
            error?.SetValue(null, null);
            initializeStdOutError?.Invoke(null, new object[] { true });

        }

        /// <summary>
        /// 
        /// </summary>
        private static void SetOutAndErroNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}
