using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class MessageSendEventArgs
    {
        private string _message;
        private long _to;
        public string Message
        {
            get { return _message; }
        }
        public long To
        {
            get { return _to; }
        }
        

        public MessageSendEventArgs(string message, long to)
        {
            _message = message;
            _to = to;
        }
    }
}
