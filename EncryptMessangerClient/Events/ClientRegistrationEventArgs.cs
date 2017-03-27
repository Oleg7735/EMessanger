using EncryptMessangerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ClientRegistrationEventArgs
    {
        /*private string _login;
        private string _password;*/
        private RegistrationInfo _registrationInfo;

        public string Login
        {
            get { return _registrationInfo.Login; }

        }
        public string Password
        {
            get { return _registrationInfo.Password; }
        }
        public ClientRegistrationEventArgs(RegistrationInfo info)
        {
            _registrationInfo = info;
        }
        public RegistrationInfo GetRegistrationInfo()
        {
            return _registrationInfo;
        }
    }
}
