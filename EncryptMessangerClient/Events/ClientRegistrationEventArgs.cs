using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class ClientRegistrationEventArgs
    {
        private string _login;
        private string _password;

        public string Login
        {
            get { return _login; }

        }
        public string Password
        {
            get { return _password; }
        }
        public ClientRegistrationEventArgs(string login, string password)
        {
            _login = login;
            _password = password;
        }
    }
}
