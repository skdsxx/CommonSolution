#region CmdHelper 文件信息
/***********************************************************
**文 件 名：CmdHelper 
**命名空间：CommonLibrary.Helpers 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2018/8/15 21:23:05 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System.Diagnostics;

namespace CommonLibrary.Helpers
{
    /// <summary>
    /// Cmd帮助类
    /// </summary>
    public class CmdHelper
    {
        /// <summary>
        /// 执行Cmd命令
        /// </summary>
        /// <param name="executePath">执行路径</param>
        /// <param name="cmd">命令</param>
        /// <returns>命令执行结果</returns>
        public static string Execute(string executePath, string cmd)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = executePath,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            p.Start();
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.WriteLine("exit");
            return p.StandardError.ReadToEnd();
        }
    }
}
