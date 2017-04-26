#region MessageDialog 文件信息
/***********************************************************
**文 件 名：MessageDialog 
**命名空间：MvvmLightTest 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2016-10-28 15:57:50 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Windows;
using System.Windows.Threading;

namespace CommonLibrary.Dialogs
{
    /// <summary>
    /// 自定义弹出消息框
    /// </summary>
    public static class MessageDialog
    {
        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="isModel">启用模态，默认启用</param>
        /// <returns></returns>
        public static DialogResult Alert(string content, bool isModel = true)
        {
            return Alert(content, "提示", isModel);
        }

        public static DialogResult Alert(string content, DialogType type,bool isModel = true)
        {
            return Alert(content, "提示",type, isModel);
        }
        public static void AlertTip(string content, bool isSuccess = true)
        {
            if (isSuccess)
            {
                var confirm = new SuccessPopWindow(content);
                confirm.Show();
                System.Threading.Thread.Sleep(1000);
                confirm.Close();
            }
            else
            {
                var confirm = new FailurePopWindow(content);
                confirm.Show();
                //阻塞了UI线程1s,实际上是不对的
                System.Threading.Thread.Sleep(1000);
                confirm.Close();
            }

        }

        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="title">消息标题</param>
        /// <param name="isModel">启用模态，默认启用</param>
        /// <returns></returns>
        public static DialogResult Alert(string content, string title, bool isModel = true)
        {
            return Alert(content, title, DialogType.Ok, isModel);
        }

       

        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="title">消息标题</param>
        /// <param name="type">对话框图标</param>
        /// <param name="isModel">启用模态，默认启用</param>
        /// <returns></returns>
        public static DialogResult Alert(string content, string title,DialogType type, bool isModel = true)
        {
            return (DialogResult)Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Func<DialogResult>(() =>
            {
                var win = new DialogWindow(content, title, type) {Owner =Application.Current.MainWindow};
                if (isModel)
                {
                    win.ShowDialog();
                }
                else
                {
                    win.Show();
                }

                return win.Result;
            }));
        }

        public static DialogResult Alert(this Window owner, string content,bool isModel = true)
        {
            return Alert(owner, content, "提示", DialogType.Ok, isModel);
        }

        public static DialogResult Alert(this Window owner,string content, DialogType type, bool isModel = true)
        {
            return Alert(owner,content, "提示", type, isModel);
        }

        public static DialogResult Alert(this Window owner, string content,string title, DialogType type=DialogType.Ok,bool isModel=true)
        {
            return (DialogResult)Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Func<DialogResult>(() =>
            {
                var win = new DialogWindow(content, title, type) { Owner = owner};
                if (isModel)
                {
                    win.ShowDialog();
                }
                else
                {
                    win.Show();
                }

                return win.Result;
            }));
           
        }
    }

    /// <summary>
    /// 窗口对话框结果
    /// </summary>
    public enum DialogResult
    {
        True,
        False
    }


    public enum DialogType
    {
        YesOrNo,
        Ok
    }
    ///// <summary>
    ///// 对话框图标
    ///// </summary>
    //[Description("")]
    //public enum DialogIcon
    //{
    //    /// <summary>
    //    /// 信息 图标
    //    /// </summary>
    //    [Description("信息 图标")]
    //    Information,

    //    /// <summary>
    //    /// 警告 图标
    //    /// </summary>
    //    [Description("警告 图标")]
    //    Warning,

    //    /// <summary>
    //    /// 错误 图标
    //    /// </summary>
    //    [Description("错误 图标")]
    //    Error,

    //    /// <summary>
    //    /// 对号 图标
    //    /// </summary>
    //    [Description("对号 图标")]
    //    CheckMark,

    //    /// <summary>
    //    /// 问号 图标
    //    /// </summary>
    //    [Description("问号 图标")]
    //    Quetion,

    //    /// <summary>
    //    /// 禁止 图标
    //    /// </summary>
    //    [Description(" 图标")]
    //    Forbidden,

    //    /// <summary>
    //    /// OK 图标
    //    /// </summary>
    //    [Description("OK 图标")]
    //    Ok,

    //    /// <summary>
    //    /// 微笑 图标
    //    /// </summary>
    //    [Description("微笑 图标")]
    //    Smile
    //}
}
