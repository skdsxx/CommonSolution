using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommonLibrary.Annotations;

namespace CommonLibrary.Dialogs
{
    /// <summary>
    /// SuccessPopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SuccessPopWindow : Window,INotifyPropertyChanged
    {

        private string _txtMessage;

        public string TxtMessage
        {
            get { return _txtMessage; }
            set
            {
                if (_txtMessage != value)
                {
                    _txtMessage = value;
                    OnPropertyChanged("TxtMessage");
                }
            }
        }
        public SuccessPopWindow(string info)
        {
            InitializeComponent();
            DataContext = this;
            TxtMessage = info;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
