using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimPanel.ViewModel
{
    public class RelayCommand : ICommand
    {
        Action<object> ExcuteAction;
        Func<object, bool> CanExecuteFunc;

        public RelayCommand(Action<object> action, Func<object, bool> can_execute = null)
        {
            this.ExcuteAction = action;
            this.CanExecuteFunc = can_execute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteFunc == null)
            {
                return true;
            }

            return CanExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            this.ExcuteAction(parameter);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        Action<object> ExcuteAction;
        Func<object, bool> CanExecuteFunc;

        public RelayCommand(Action<object> action, Func<object, bool> can_execute = null)
        {
            this.ExcuteAction = action;
            this.CanExecuteFunc = can_execute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteFunc == null)
            {
                return true;
            }

            return CanExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            this.ExcuteAction(parameter);
        }
    }
}
