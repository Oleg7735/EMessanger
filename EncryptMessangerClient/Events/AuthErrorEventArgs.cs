using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class AuthErrorEventArgs
    {
        private string _error;
        public string Error
        {
            get
            {
                return _error;
            }
        }
        public AuthErrorEventArgs(string error)
        {
            _error = error;
        }
    }
}
