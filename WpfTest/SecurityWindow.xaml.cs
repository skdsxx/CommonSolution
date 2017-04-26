using System.Windows;
using CommonLibrary.SecurityHelper;

namespace WpfTest
{
    /// <summary>
    /// SecurityWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SecurityWindow : Window
    {
        public SecurityWindow()
        {
            InitializeComponent();
        }

        private void ButtonDesEncript_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NormalText.Text.Trim()))
            {
                MessageBox.Show("加密文本不能为空");
                return;
            }
            DesEncryptText.Text=SecurityHelper.EncryptDes(NormalText.Text.Trim());
        }

        private void ButtonDesDecript(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DesEncryptText.Text.Trim()))
            {
                MessageBox.Show("解密文本不能为空");
                return;
            }
            DesDecryptText.Text = SecurityHelper.DecryptDes(DesEncryptText.Text.Trim());
        }

        private void ButtonMd5Encript_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Md5NormalText.Text.Trim()))
            {
                MessageBox.Show("加密文本不能为空");
                return;
            }
            Md5EncryptText.Text = SecurityHelper.EncodeByMd532(Md5NormalText.Text.Trim());
        }
    }
}
