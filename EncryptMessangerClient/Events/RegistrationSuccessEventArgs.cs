using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class RegistrationSuccessEventArgs
    {
        //Логин, под которым пользователь идентифицирован на сервере
        private string _login;

        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
            }
        }
        public RegistrationSuccessEventArgs(string login)
        {
            Login = login;
        }
    }
}
