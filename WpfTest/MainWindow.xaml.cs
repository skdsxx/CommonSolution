using System.Windows;

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
    }
}
