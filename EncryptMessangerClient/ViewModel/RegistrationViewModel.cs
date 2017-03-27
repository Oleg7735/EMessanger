using EncryptMessangerClient.Events;
using EncryptMessangerClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.ViewModel
{
    class RegistrationViewModel : INotifyPropertyChanged
    {
        /*private string _login;
        private string _password;*/
        private string _passwordConfirm;
        private string _error;

        public Command RegistrationCommand { get; set; }
        public Command CanselCommand { get; set; }

        public event EventHandler<ClientRegistrationEventArgs> RegistrationEventHandler;
        public event EventHandler CanselEventHandler;

        private RegistrationInfo _registrationInfo;

        public string Login
        {
            get
            {
                return _registrationInfo.Login;                
            }

            set
            {
                if (!String.IsNullOrWhiteSpace(value) && !value.Equals(_registrationInfo.Login))
                {
                    _registrationInfo.Login = value;
                    RegistrationCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get
            {
                return _registrationInfo.Password;
            }

            set
            {
                if (!String.IsNullOrWhiteSpace(value) && !value.Equals(_registrationInfo.Password))
                {
                    _registrationInfo.Password = value;
                    RegistrationCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string PasswordConfirm
        {
            get
            {
                return _passwordConfirm;
            }

            set
            {
                if (!String.IsNullOrWhiteSpace(value) && !value.Equals(_passwordConfirm))
                {
                    _passwordConfirm = value;
                    RegistrationCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }

            set
            {
                if(!value.Equals(_error)&&!String.IsNullOrEmpty(value))
                _error = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Registrate()
        {
            RegistrationEventHandler?.Invoke(this, new ClientRegistrationEventArgs(_registrationInfo));
        }
        private bool CanRegistrate()
        {
            return (!String.IsNullOrWhiteSpace(Login)) && (!String.IsNullOrWhiteSpace(Password)) && 
                (!String.IsNullOrWhiteSpace(_passwordConfirm))&&(Password.Equals(_passwordConfirm));
        }

        private void Cansel()
        {            
            CanselEventHandler?.Invoke(this, EventArgs.Empty);
        }
        private bool CanCansel()
        {
            return true;
        }

        public RegistrationViewModel()
        {
            RegistrationCommand = new Command(Registrate, CanRegistrate);
            CanselCommand = new Command(Cansel, CanCansel);
            _registrationInfo = new RegistrationInfo();
        }
    }
}
