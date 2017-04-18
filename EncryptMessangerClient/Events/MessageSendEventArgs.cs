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
        private long _dialogId;
        public string Message
        {
            get { return _message; }
        }
        public long DialogId
        {
            get { return _dialogId; }
        }
        

        public MessageSendEventArgs(string message, long dialogId)
        {
            _message = message;
            _dialogId = dialogId;
        }
    }
}
