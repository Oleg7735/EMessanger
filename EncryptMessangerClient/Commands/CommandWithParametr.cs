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
        private Action _canEecute;
        private Action<long> _execute;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //return _canEecute.Invoke();
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
