#region EventHelper 文件信息
/***********************************************************
**文 件 名：EventHelper 
**命名空间：CommonLibrary.Helpers 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017-02-24 13:47:30 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;

namespace CommonLibrary.Helpers
{
    public class EventHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        public static void BeginRaise(EventHandler handler, object sender, AsyncCallback callback, object asyncState)
        {
            if (handler != null)
            {
                new Action<EventHandler, object>(Raise).BeginInvoke(handler, sender, callback, asyncState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        public static void BeginRaise(Delegate handler, object sender, EventArgs e, AsyncCallback callback, object asyncState)
        {
            if (handler != null)
            {
                new Action<Delegate, object, EventArgs>(Raise).BeginInvoke(handler, sender, e, callback, asyncState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        public static void BeginRaise<T>(EventHandler<T> handler, object sender, T e, AsyncCallback callback, object asyncState) where T : EventArgs
        {
            if (handler != null)
            {
                new Action<EventHandler<T>, object, T>(Raise).BeginInvoke(handler, sender, e, callback, asyncState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="createEventArguments"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        public static void BeginRaise<T>(EventHandler<T> handler, object sender, CreateEventArguments<T> createEventArguments, AsyncCallback callback, object asyncState) where T : EventArgs
        {
            ArgumentHelper.AssertNotNull(createEventArguments, "createEventArguments");
            if (handler != null)
            {
                new Action<EventHandler<T>, object, CreateEventArguments<T>>(Raise).BeginInvoke(handler, sender, createEventArguments, callback, asyncState);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        public static void Raise(EventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Raise(Delegate handler, object sender, EventArgs e)
        {
            if (handler != null)
            {
                handler.DynamicInvoke(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Raise<T>(EventHandler<T> handler, object sender, T e) where T : EventArgs
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="createEventArguments"></param>
        public static void Raise<T>(EventHandler<T> handler, object sender, CreateEventArguments<T> createEventArguments) where T : EventArgs
        {
            ArgumentHelper.AssertNotNull(createEventArguments, "createEventArguments");
            if (handler != null)
            {
                handler(sender, createEventArguments());
            }
        }

        public delegate T CreateEventArguments<T>() where T : EventArgs;
    }
}
