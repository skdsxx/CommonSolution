#region DialogWindow 文件信息
/***********************************************************
**文 件 名：DialogWindow 
**命名空间：MvvmLightTest.Dialogs 
**内     容： 
**功     能： 
**文件关系： 
**作     者：LvJunlei
**创建日期：2016-10-28 15:15:36 
**版 本 号：V1.0.0.0 
**修改日志： 
**版权说明： 
************************************************************/
#endregion

using System.ComponentModel;
using System.Windows.Input;

namespace CommonLibrary.Dialogs
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : INotifyPropertyChanged
    {
        public DialogWindow() : this("", "")
        {
        }

        public DialogWindow(string content, string title, DialogType dialogType = DialogType.Ok)
        {
            InitializeComponent();
            DataContext = this;
            DialogTitle = title;
            DialogContent = content;

            if (dialogType== DialogType.YesOrNo)
            {
                ButtonNo.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonNo.Visibility = Visibility.Collapsed;
                ButtonOk.Content = "确定";
            }
            
        }

        //更改Enter按钮响应事件，使界面控件能够按照TabIndex的大小顺序获取焦点
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // MoveFocus takes a TraveralReqest as its argument.
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            Result = DialogResult.True;
        }

        private void ButtonNo_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            Result = DialogResult.False;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogResult Result { get; set; }

        #region 依赖属性

        private string _dialogTitle;

        /// <summary>
        /// 消息标题
        /// </summary>
        public string DialogTitle
        {
            get { return _dialogTitle; }
            set
            {
                if (_dialogTitle != value)
                {
                    _dialogTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _dialogContent;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string DialogContent
        {
            get { return _dialogContent; }
            set
            {
                if (_dialogContent != value)
                {
                    _dialogContent = value;
                    OnPropertyChanged();
                }
            }
        }

        //private string _dialogIcon;

        ///// <summary>
        ///// 对话框标题
        ///// </summary>
        //public string DialogIcon
        //{
        //    get { return _dialogIcon; }
        //    set
        //    {
        //        if (_dialogIcon != value)
        //        {
        //            _dialogIcon = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
