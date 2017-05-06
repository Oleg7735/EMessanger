using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EncryptMessangerClient.Commands
{
    class CommandWithParametr : ICommand
    {
        private Action<object> _targetExecuteAction;
        private Func<bool> _targetCanExecuteMethod;
        public event EventHandler CanExecuteChanged;

        public CommandWithParametr(Action<object> executeAction)
        {
            _targetExecuteAction = executeAction;
        }

        public CommandWithParametr(Action<object> executeAction, Func<bool> canExecuteMethod)
        {
            _targetExecuteAction = executeAction;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (_targetCanExecuteMethod != null)
            {
                return _targetCanExecuteMethod();
            }

            return _targetExecuteAction != null;
        }

        public void Execute(object parameter)
        {
            _targetExecuteAction?.Invoke(parameter);
        }
    }
}
