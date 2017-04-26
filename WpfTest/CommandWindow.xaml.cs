using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommonLibrary.Command;
using WpfTest.Annotations;

namespace WpfTest
{
    /// <summary>
    /// CommandWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CommandWindow : Window, INotifyPropertyChanged
    {
        private string _contentShow;

        public string ContentShow
        {
            get { return _contentShow; }
            set
            {
                if (_contentShow != value)
                {
                    _contentShow = value;
                    OnPropertyChanged(nameof(ContentShow));
                }
            }
        }

        public ICommand BtnTextCommand { get; set; }
        public ICommand BtnTextParaCommand { get; set; }
        public CommandWindow()
        {
            InitializeComponent();
            DataContext = this;
            BtnTextCommand = new RelayCommand(BtnText);
            BtnTextParaCommand = new RelayCommand<string>(BtnTextPara);
        }


        private void BtnText()
        {
            ContentShow += "你好呀，夏天;";
        }
        private void BtnTextPara(string str)
        {
            ContentShow += str;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
