using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Model
{
    public class RegistrationInfo : System.ComponentModel.INotifyPropertyChanged
    {
        private string _login;
        private string _password;

        public RegistrationInfo()
        {
        }

        public RegistrationInfo(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                if (!value.Equals(Login)&& value != null)
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
                if (!value.Equals(_password) && value != null)
                {
                    _password = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
