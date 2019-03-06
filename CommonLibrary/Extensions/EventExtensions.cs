#region EventExtensions 文件信息
/***********************************************************
**文 件 名：EventExtensions 
**命名空间：CommonLibrary.Extensions 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017-02-24 13:55:34 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Diagnostics;
using CommonLibrary.Helpers;

namespace CommonLibrary.Extensions
{
    public static  class EventExtensions
    {
        [DebuggerHidden]
        public static void BeginRaise(this EventHandler handler, object sender, AsyncCallback callback, object asyncState)
        {
            EventHelper.BeginRaise(handler, sender, callback, asyncState);
        }

        [DebuggerHidden, Obsolete("This API will be removed in a future version of The Helper Trinity.")]
        public static void BeginRaise(this Delegate handler, object sender, EventArgs e, AsyncCallback callback, object asyncState)
        {
            EventHelper.BeginRaise(handler, sender, e, callback, asyncState);
        }

        [DebuggerHidden]
        public static void BeginRaise<T>(this EventHandler<T> handler, object sender, T e, AsyncCallback callback, object asyncState) where T : EventArgs
        {
            EventHelper.BeginRaise(handler, sender, e, callback, asyncState);
        }

        [DebuggerHidden]
        public static void BeginRaise<T>(this EventHandler<T> handler, object sender, EventHelper.CreateEventArguments<T> createEventArguments, AsyncCallback callback, object asyncState) where T : EventArgs
        {
            EventHelper.BeginRaise(handler, sender, createEventArguments, callback, asyncState);
        }

        [DebuggerHidden]
        public static void Raise(this EventHandler handler, object sender)
        {
            EventHelper.Raise(handler, sender);
        }

        [DebuggerHidden, Obsolete("This API will be removed in a future version of The Helper Trinity.")]
        public static void Raise(this Delegate handler, object sender, EventArgs e)
        {
            EventHelper.Raise(handler, sender, e);
        }

        [DebuggerHidden]
        public static void Raise<T>(this EventHandler<T> handler, object sender, T e) where T : EventArgs
        {
            EventHelper.Raise(handler, sender, e);
        }

        [DebuggerHidden]
        public static void Raise<T>(this EventHandler<T> handler, object sender, EventHelper.CreateEventArguments<T> createEventArguments) where T : EventArgs
        {
            EventHelper.Raise(handler, sender, createEventArguments);
        }
    }
}
