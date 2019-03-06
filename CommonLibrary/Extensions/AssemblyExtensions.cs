#region 文件和作者信息
/******************************************************************

  * CLR 版本: 4.0.30319.42000
  * 机器名称: SUNXIAN
  * 命名空间: CommonLibrary.Extensions
  * 文 件 名: AssemblyExtensions
  * 创建人员: SunXian
  * 创建时间: 2017/4/26 10:37:54
  
//******************************************************************
* Copyright： @青岛凯亚研发部@
******************************************************************/
#endregion

namespace CommonLibrary.Extensions
{
    public static class AssemblyExtensions
    {
        #region GetFileVersion(获取程序集的文件版本号)
        /// <summary>
        /// 获取程序集的文件版本号
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <returns>程序集文件版本号</returns>
        //public static Version GetFileVersion(this Assembly assembly)
        //{
        //    assembly.CheckNotNull("assembly");
        //    FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
        //    return new Version(info.FileVersion);
        //}
        #endregion

        #region GetProductVersion(获取程序集的产品版本)
        /// <summary>
        /// 获取程序集的产品版本
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <returns>程序集产品版本</returns>
        //public static Version GetProductVersion(this Assembly assembly)
        //{
        //    assembly.CheckNotNull("assembly");
        //    FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
        //    return new Version(info.ProductVersion);
        //}
        #endregion

        /// <summary>
        /// 获取当前系统的版本号
        /// </summary>
        /// <returns>版本号</returns>
        public static string GetEdition()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
