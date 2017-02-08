using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ClientStatusOnlineEventArgs
    {
        private string[] _logins;
        public string[] Logins
        {
            get { return _logins; }
        }
        public ClientStatusOnlineEventArgs(string[] logins)
        {
            _logins = logins;
        }
    }
}
