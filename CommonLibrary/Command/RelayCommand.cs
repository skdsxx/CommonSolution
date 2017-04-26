#region 文件和作者信息
/******************************************************************

  * CLR 版本: 4.0.30319.42000
  * 机器名称: SUNXIAN
  * 命名空间: CommonLibrary.Command
  * 文 件 名: RelayCommand
  * 创建人员: SunXian
  * 创建时间: 2017/4/26 10:23:37
  
//******************************************************************
* Copyright： @青岛凯亚研发部@
******************************************************************/
#endregion

using System;
using System.Windows.Input;

namespace CommonLibrary.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute = null;
        private readonly Func<bool> _canExecute = null;

        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            return true;
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute();
            }
        }

        public event EventHandler CanExecuteChanged;
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute ?? (x => true);
        }

        public bool CanExecute(T parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;
        }

        public void Execute(T parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
        }

        #region ICommand 成员

        public bool CanExecute(object parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
            {
                return (_canExecute == null);
            }

            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        #endregion

        public event EventHandler CanExecuteChanged;
        //{
        //    add
        //    {
        //        CommandManager.RequerySuggested += value;
        //    }
        //    remove
        //    {
        //        CommandManager.RequerySuggested -= value;
        //    }
        //}

        //public void Refresh()
        //{
        //    CommandManager.InvalidateRequerySuggested();
        //}
    }
}
