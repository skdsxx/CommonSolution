using System.Windows;
using CommonLibrary.Dialogs;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSecurity_OnClick(object sender, RoutedEventArgs e)
        {
            new SecurityWindow().Show();
        }

        private void ButtonCommand_OnClick(object sender, RoutedEventArgs e)
        {
            
            new CommandWindow().Show();
        }

        private void ButtonMessageOk_OnClick(object sender, RoutedEventArgs e)
        {
            MessageDialog.Alert(TxtMessageOk.Text);
        }

        private void ButtonMessagePop_OnClick(object sender, RoutedEventArgs e)
        {
            MessageDialog.AlertTip(TxtMessagePop.Text);
        }

        private void ButtonMessageOkCancel_OnClick(object sender, RoutedEventArgs e)
        {
            MessageDialog.Alert(TxtMessageOkCancel.Text,DialogType.YesOrNo);
        }
    }
}
