using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class SendFileEventArgs:EventArgs
    {
        string _filePath;
        string _fileName;
        long _dialogId;
        long _senderId;

        public SendFileEventArgs(string filePath, string fileName, long dialogId, long senderId)
        {
            _filePath = filePath;
            _fileName = fileName;
            _dialogId = dialogId;
            _senderId = senderId;
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }

            set
            {
                _filePath = value;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
            }
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

        public long SenderId
        {
            get
            {
                return _senderId;
            }

            set
            {
                _senderId = value;
            }
        }
    }
}
