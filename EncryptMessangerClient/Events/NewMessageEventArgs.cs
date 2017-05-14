using EncryptMessangerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient
{
    public class NewMessageEventArgs : EventArgs
    {
        private long _messageId;
        private string _message;
        private long _from;
        private long _dialogId;
        private DateTime _sendDate;
        private string _error = "";
        private bool _hasAttach = false;
        private long _attachId;
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

        public DateTime SendDate
        {
            get
            {
                return _sendDate;
            }

            set
            {
                _sendDate = value;
            }
        }

        public long MessageId
        {
            get
            {
                return _messageId;
            }

            set
            {
                _messageId = value;
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }

            set
            {
                _error = value;
            }
        }

        public bool HasAttach
        {
            get
            {
                return _hasAttach;
            }

            set
            {
                _hasAttach = value;
            }
        }

        public long AttachId
        {
            get
            {
                return _attachId;
            }

            set
            {
                _attachId = value;
            }
        }

        public NewMessageEventArgs(long messageId, string message, long dialogId, long from, DateTime sendDate, bool isAltered, string error = "")
        {
            _message = message;
            _from = from;
            _isAltered = isAltered;
            _dialogId = dialogId;
            MessageId = messageId;
            SendDate = sendDate;
            Error = error;
        }
        public NewMessageEventArgs(long messageId, string message, long dialogId, long from, DateTime sendDate, bool isAltered, long attachId, string error = "")
        {
            _message = message;
            _from = from;
            _isAltered = isAltered;
            _dialogId = dialogId;
            MessageId = messageId;
            SendDate = sendDate;
            HasAttach = true;
            AttachId = attachId;
            Error = error;
        }
        

    }
}
