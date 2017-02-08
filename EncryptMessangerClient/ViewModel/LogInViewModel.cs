using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EncryptMessangerClient.Events;

namespace EncryptMessangerClient.ViewModel
{
    class LogInViewModel : INotifyPropertyChanged
    {
        public event EventHandler<ClientAuthEventArgs> AuthClient;
        public event EventHandler CloseClient;
        public event EventHandler<ClientRegistrationEventArgs> RegistrateClient;
        public Command ClientAuthCommand { get; private set; }
        public Command ClientCloseCommand { get; private set; }

        public LogInViewModel()
        {
            //инициализация команд аутентификации и выхода
            ClientAuthCommand = new Command(Auth, CanAuth);
            ClientCloseCommand = new Command(Exit,CanExit);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _login;
        private string _password;
        private string _authError;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                if (value != null && !value.Equals(_login))
                {
                    _login = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != null && !value.Equals(_password))
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }
        //ошибка аутентификации, выводимая пользователю
        public string AuthError
        {
            get { return _authError; }
            set
            {
                if (value != null && !value.Equals(_authError))
                {
                    _authError = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Auth()
        {
            if (_login != null && _password != null)
            {
                AuthClient?.Invoke(this, new ClientAuthEventArgs(_login, _password));
            }
        }
        private bool CanAuth()
        {
            return true;
        }
        private void Exit()
        {
            CloseClient?.Invoke(this, EventArgs.Empty);
        }
        private bool CanExit()
        {
            return true;
        }
        private void Registrate()
        {

        }
        private bool CanRegistrate()
        {
            return true;
        }
    }
}
