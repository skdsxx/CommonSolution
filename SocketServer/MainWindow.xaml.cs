
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SocketServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 监听线程
        /// </summary>
        Thread threadWatch = null;

        /// <summary>
        /// 监听Socket
        /// </summary>
        Socket socketWatch = null;

        /// <summary>
        /// 连接客户端Socket缓存字典表
        /// </summary>
        Dictionary<string, Socket> dict = new Dictionary<string, Socket>();

        /// <summary>
        /// 连接客户端Socket线程缓存字典表
        /// </summary>
        Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(TxtServerIp.Text.Trim());
            IPEndPoint endpoint = new IPEndPoint(address, int.Parse(TxtServerPort.Text.Trim()));
            socketWatch.Bind(endpoint);//绑定Ip和端口
            socketWatch.Listen(10);//设置最多10个排队连接请求  
            threadWatch = new Thread(WatchConnection);
            threadWatch.IsBackground = true;
            threadWatch.Start();
            ShowMsg("服务器监听成功......");
        }

        /// <summary>
        /// 监听客户端的请求方法
        /// </summary>
        /// <param name="obj"></param>
        private void WatchConnection(object obj)
        {
            while (true)
            {
                // 负责通信的套接字
                Socket sokConnection = socketWatch.Accept();
                LbOnline.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () {
                    LbOnline.Items.Add(sokConnection.RemoteEndPoint.ToString());//界面显示在线客户端段
                });
                dict.Add(sokConnection.RemoteEndPoint.ToString(), sokConnection);//在线客户端段Socket缓存添加

                //创建通信线程
                ParameterizedThreadStart pts = new ParameterizedThreadStart(RecMsg);// 委托
                Thread thr = new Thread(pts);
                thr.IsBackground = true;
                thr.Start(sokConnection);
                dictThread.Add(sokConnection.RemoteEndPoint.ToString(), thr);

                ShowMsg("客户端连接成功......" + sokConnection.RemoteEndPoint.ToString());
            }
        }

        /// <summary>
        /// 服务端负责监听客户端发送消息的方法
        /// </summary>
        void RecMsg(object socketClient)
        {
            Socket socketClients = socketClient as Socket;
            while (true)
            {
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];

                int length = -1;
                try
                {
                    length = socketClients.Receive(arrMsgRec);
                }
                catch (SocketException ex)
                {
                    ShowMsg("异常：" + ex.Message);
                    // 从 通信套接字 集合中删除被中断连接的套接字对象
                    dict.Remove(socketClients.RemoteEndPoint.ToString());
                    // 从 通信线程 集合中删除被中断连接的套接字对象
                    dictThread.Remove(socketClients.RemoteEndPoint.ToString());
                    // 从 列表 中移除 IP&Port
                    LbOnline.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () {
                        LbOnline.Items.Remove(socketClients.RemoteEndPoint.ToString());
                    });
                    
                    break;
                }
                catch (Exception ex)
                {
                    ShowMsg("异常：" + ex.Message);
                    break;
                }
                // 判断第一个发送过来的数据如果是1，则代表发送过来的是文本数据
                if (arrMsgRec[0] == 0)
                {
                    string strMsgRec = System.Text.Encoding.UTF8.GetString(arrMsgRec, 1, length - 1);
                    ShowMsg(strMsgRec);
                }
                // 若是1则发送过来的是文件数据（文档，图片，视频等。。。）
                else if (arrMsgRec[0] == 1)
                {
                    // 保存文件选择框对象
                    SaveFileDialog sfd = new SaveFileDialog();
                    // 选择文件路径
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // 获得保存文件的路径
                        string fileSavePath = sfd.FileName;
                        // 创建文件流，然后让文件流根据路径创建一个文件
                        using (FileStream fs = new FileStream(fileSavePath, FileMode.Create))
                        {
                            fs.Write(arrMsgRec, 1, length - 1);
                            ShowMsg("文件保存成功：" + fileSavePath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 显示进度信息
        /// </summary>
        /// <param name="str"></param>
        private void ShowMsg(string str)
        {
            TxtMsg.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate(){
                TxtMsg.AppendText(str + "\r\n");
            });
        }

        /// <summary>
        /// 信息发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (LbOnline.SelectedItem==null)
            {
                System.Windows.MessageBox.Show("请选择发送的好友！！！");
            }
            else
            {
                string strMsg = TxtSendMsg.Text.Trim();
                byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
                string strClientKey = LbOnline.SelectedItem.ToString();
                try
                {
                    dict[strClientKey].Send(arrMsg,SocketFlags.None);
                    ShowMsg("发送了数据出去：" + strMsg);
                }
                catch (SocketException ex)
                {
                    ShowMsg("发送时异常：" + ex.Message);
                }
                catch (Exception ex)
                {
                    ShowMsg("发送时异常：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 群发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendTpAll_Click(object sender, RoutedEventArgs e)
        {
            string strMsg = TxtSendMsg.Text.Trim();
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
            foreach (Socket item in dict.Values)
            {
                item.Send(arrMsg);
            }
            ShowMsg("群发完毕！！！:）");
        }
    }
}
