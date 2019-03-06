#region ExceptionHelper 文件信息
/***********************************************************
**文 件 名：ExceptionHelper 
**命名空间：CommonLibrary.Helpers 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2017-02-24 13:49:35 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using CommonLibrary.Extensions;

namespace CommonLibrary.Helpers
{
    public class ExceptionHelper
    {
        private static readonly IDictionary<ExceptionInfoKey, XDocument> exceptionInfos = new Dictionary<ExceptionInfoKey, XDocument>();
        private static readonly object exceptionInfosLock = new object();
        private readonly Type forType;
        private readonly string resourceName;
        private const string typeAttributeName = "type";

        public ExceptionHelper(Type forType) : this(forType, null, 0)
        {
        }

        public ExceptionHelper(Type forType, string resourceName) : this(forType, resourceName, 0)
        {
            resourceName.AssertNotNullOrEmpty("resource", true);
        }

        private ExceptionHelper(Type forType, string resourceName, int dummy)
        {
            forType.AssertNotNull<Type>("forType");
            this.forType = forType;
            if (resourceName != null)
            {
                this.resourceName = resourceName;
            }
            else
            {
                this.resourceName = forType.Assembly.GetName().Name + ".Properties.ExceptionHelper.xml";
            }
        }

        [DebuggerHidden]
        private static XDocument GetExceptionInfo(Assembly assembly, string resourceName)
        {
            XDocument document = null;
            var key = new ExceptionInfoKey(assembly, resourceName);
            lock (exceptionInfosLock)
            {
                if (exceptionInfos.ContainsKey(key))
                {
                    return exceptionInfos[key];
                }
                var manifestResourceStream = assembly.GetManifestResourceStream(resourceName);
                if (manifestResourceStream == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "XML resource file '{0}' could not be found in assembly '{1}'.", new object[] { resourceName, assembly.FullName }));
                }
                using (var reader = new StreamReader(manifestResourceStream))
                {
                    document = XDocument.Load(reader);
                }
                exceptionInfos[key] = document;
            }
            return document;
        }

        [DebuggerHidden]
        public Exception Resolve(string exceptionKey, params object[] messageArgs)
        {
            return this.Resolve(exceptionKey, null, null, messageArgs);
        }

        [DebuggerHidden]
        public Exception Resolve(string exceptionKey, Exception innerException, params object[] messageArgs)
        {
            return this.Resolve(exceptionKey, null, innerException, messageArgs);
        }

        [DebuggerHidden]
        public Exception Resolve(string exceptionKey, object[] constructorArgs, Exception innerException)
        {
            return this.Resolve(exceptionKey, constructorArgs, innerException, null);
        }

        [DebuggerHidden]
        public Exception Resolve(string exceptionKey, object[] constructorArgs, params object[] messageArgs)
        {
            return this.Resolve(exceptionKey, constructorArgs, null, messageArgs);
        }

        [DebuggerHidden]
        public Exception Resolve(string exceptionKey, object[] constructorArgs, Exception innerException, params object[] messageArgs)
        {
            exceptionKey.AssertNotNull<string>("exceptionKey");
            var element = (from exceptionGroup in GetExceptionInfo(this.forType.Assembly, this.resourceName).Element("exceptionHelper").Elements("exceptionGroup")
                                from exception in exceptionGroup.Elements("exception")
                                where string.Equals(exceptionGroup.Attribute("type").Value, this.forType.FullName, StringComparison.Ordinal) && string.Equals(exception.Attribute("key").Value, exceptionKey, StringComparison.Ordinal)
                                select exception).FirstOrDefault<XElement>();
            if (element == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The exception details for key '{0}' could not be found at /exceptionHelper/exceptionGroup[@type'{1}']/exception[@key='{2}'].", new object[] { exceptionKey, this.forType, exceptionKey }));
            }
            var attribute = element.Attribute("type");
            if (attribute == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The '{0}' attribute could not be found for exception with key '{1}'", new object[] { "type", exceptionKey }));
            }
            var c = Type.GetType(attribute.Value);
            if (c == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' could not be loaded for exception with key '{1}'", new object[] { attribute.Value, exceptionKey }));
            }
            if (!typeof(Exception).IsAssignableFrom(c))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' for exception with key '{1}' does not inherit from '{2}'", new object[] { c.FullName, exceptionKey, typeof(Exception).FullName }));
            }
            var format = element.Value.Trim();
            if ((messageArgs != null) && (messageArgs.Length > 0))
            {
                format = string.Format(CultureInfo.InvariantCulture, format, messageArgs);
            }
            var list = new List<object> {
                format
            };
            if (constructorArgs != null)
            {
                list.AddRange(constructorArgs);
            }
            if (innerException != null)
            {
                list.Add(innerException);
            }
            var args = list.ToArray();
            var bindingAttr = BindingFlags.Public | BindingFlags.Instance;
            ConstructorInfo info = null;
            try
            {
                object obj2;
                info = (ConstructorInfo)Type.DefaultBinder.BindToMethod(bindingAttr, c.GetConstructors(bindingAttr), ref args, null, null, null, out obj2);
            }
            catch (MissingMethodException)
            {
            }
            if (info == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "An appropriate constructor could not be found for exception type '{0}, for exception with key '{1}'", new object[] { c.FullName, exceptionKey }));
            }
            return (Exception)info.Invoke(args);
        }

        [DebuggerHidden]
        public void ResolveAndThrowIf(bool condition, string exceptionKey, params object[] messageArgs)
        {
            if (condition)
            {
                throw this.Resolve(exceptionKey, messageArgs);
            }
        }

        [DebuggerHidden]
        public void ResolveAndThrowIf(bool condition, string exceptionKey, Exception innerException, params object[] messageArgs)
        {
            if (condition)
            {
                throw this.Resolve(exceptionKey, innerException, messageArgs);
            }
        }

        [DebuggerHidden]
        public void ResolveAndThrowIf(bool condition, string exceptionKey, object[] constructorArgs, Exception innerException)
        {
            if (condition)
            {
                throw this.Resolve(exceptionKey, constructorArgs, innerException);
            }
        }

        [DebuggerHidden]
        public void ResolveAndThrowIf(bool condition, string exceptionKey, object[] constructorArgs, params object[] messageArgs)
        {
            if (condition)
            {
                throw this.Resolve(exceptionKey, constructorArgs, messageArgs);
            }
        }

        [DebuggerHidden]
        public void ResolveAndThrowIf(bool condition, string exceptionKey, object[] constructorArgs, Exception innerException, params object[] messageArgs)
        {
            if (condition)
            {
                throw this.Resolve(exceptionKey, constructorArgs, innerException, messageArgs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct ExceptionInfoKey : IEquatable<ExceptionHelper.ExceptionInfoKey>
        {
            private readonly Assembly assembly;
            private readonly string resourceName;
            public ExceptionInfoKey(Assembly assembly, string resourceName)
            {
                this.assembly = assembly;
                this.resourceName = resourceName;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(ExceptionHelper.ExceptionInfoKey other)
            {
                return (other.assembly.Equals(this.assembly) && string.Equals(other.resourceName, this.resourceName, StringComparison.Ordinal));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return ((obj is ExceptionHelper.ExceptionInfoKey) && this.Equals((ExceptionHelper.ExceptionInfoKey)obj));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                var num = 0x11;
                num = (num * 0x17) + this.assembly.GetHashCode();
                return ((num * 0x17) + this.resourceName.GetHashCode());
            }
        }
    }
}
