using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient
{
    public class NewMessageEventArgs : EventArgs
    {
        private string _message;
        private string _to;
        public string Message
        {
            get { return _message; }
        }
        public string Resiver
        {
            get { return _to; }
        }

        public NewMessageEventArgs(string message, string to)
        {
            _message = message;
            _to = to;
        }
    }
}
