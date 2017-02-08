using EncryptMessangerClient.Events;
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
        private string _login;
        private string _password;
        private string _passwordConfirm;

        public Command RegistrationCommand { get; set; }
        public Command CanselCommand { get; set; }

        public event EventHandler<ClientAuthEventArgs> RegistrationEventHandler;
        public event EventHandler CanselEventHandler;

        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                if (!String.IsNullOrWhiteSpace(value) && !value.Equals(_login))
                {
                    _login = value;
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
                if (!String.IsNullOrWhiteSpace(value) && !value.Equals(_password))
                {
                    _password = value;
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
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Registrate()
        {
            RegistrationEventHandler?.Invoke(this, new ClientAuthEventArgs(_login, _password));
        }
        private bool CanRegistrate()
        {
            return (!String.IsNullOrWhiteSpace(_login)) && (!String.IsNullOrWhiteSpace(_password)) && 
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
        }
    }
}
