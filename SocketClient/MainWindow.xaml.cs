using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SocketClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread threadRec = null;
        Socket socketClient = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 初始化连接
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            IPAddress address = IPAddress.Parse(TxtServerIp.Text.Trim());
            IPEndPoint endport = new IPEndPoint(address, int.Parse(TxtServerPort.Text.Trim()));
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(endport);

            threadRec = new Thread(RecMsg);
            threadRec.IsBackground = true;
            threadRec.Start();
        }
        void RecMsg()
        {
            while (true)
            {
                try
                {
                    byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                    int length = socketClient.Receive(arrMsgRec);
                    string strMsgRec = System.Text.Encoding.UTF8.GetString(arrMsgRec, 0, length);
                    ShowMsg(strMsgRec);
                }
                catch(SocketException ex)
                {
                    socketClient.Close();
                    ShowMsg(ex.ToString());
                }
            }
        }
        #endregion

        private void ShowMsg(string msg)
        {
            TxtMsg.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () {
                TxtMsg.AppendText(msg + "\r\n");
            });
        }

        private void BtnSendMsg_Click(object sender, RoutedEventArgs e)
        {
            var strMsg = "";
            TxtSendMsg.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () {
                strMsg = TxtSendMsg.Text.Trim();
            });
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
            byte[] arrMsgSend = new byte[arrMsg.Length+1];
            // 添加标识位，0代表发送的是文字
            arrMsgSend[0] = 0;
            Buffer.BlockCopy(arrMsg, 0, arrMsgSend, 1, arrMsg.Length);
            socketClient.Send(arrMsgSend);
            ShowMsg("I say:" + strMsg);
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtFileName.Text = ofd.FileName;
            }
        }

        private void BtnSendFile_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(TxtFileName.Text.Trim()))
            {
                return;
            }
            using (FileStream fs = new FileStream(TxtFileName.Text.Trim(),FileMode.Open))
            {
                byte[] arrFile = new byte[1024 * 1024 * 2];
                int length = fs.Read(arrFile, 0, arrFile.Length);
                byte[] arrFileSend = new byte[length + 1];
                arrFileSend[0] = 1;// 代表文件数据
                // 将数组 arrFile 里的数据从第零个数据拷贝到 数组 arrFileSend 里面，从第1个开始，拷贝length个数据
                Buffer.BlockCopy(arrFile, 0, arrFileSend, 1, length);
                socketClient.Send(arrFileSend);
            }
        }
    }
}
