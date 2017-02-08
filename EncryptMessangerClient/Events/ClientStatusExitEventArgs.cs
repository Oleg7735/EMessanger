using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ClientStatusExitEventArgs
    {
        private string _login;
        public string Login
        {
            get { return _login; }
        }
        public ClientStatusExitEventArgs(string login)
        {
            _login = login;
        }
    }
}
