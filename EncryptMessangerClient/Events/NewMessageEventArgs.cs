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
        private long _from;
        private long _dialogId;
        public string Message
        {
            get { return _message; }
        }
        public long Interlocutor
        {
            get { return _from; }
        }
        private bool _isAltered;
        public bool IsAltered
        {
            get { return _isAltered; }
        }

        public long DialogId
        {
            get
            {
                return _dialogId;
            }

            set
            {
                _dialogId = value;
            }
        }

        public NewMessageEventArgs(string message, long dialogId, long from, bool isAltered)
        {
            _message = message;
            _from = from;
            _isAltered = isAltered;
            _dialogId = dialogId;
        }

    }
}
