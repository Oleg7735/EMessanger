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
        private string _from;
        public string Message
        {
            get { return _message; }
        }
        public string Interlocutor
        {
            get { return _from; }
        }
        private bool _isAltered;
        public bool IsAltered
        {
            get { return _isAltered; }
        }

        public NewMessageEventArgs(string message, string from, bool isAltered)
        {
            _message = message;
            _from = from;
            _isAltered = isAltered;
        }

    }
}
