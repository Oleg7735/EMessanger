using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class LoadDialogSessionEventArgs:EventArgs
    {
        private long _dialogId;
        private bool _encryptMessages;
        private bool _signMessages;
        public LoadDialogSessionEventArgs(long dialogId, bool encryptMessages, bool signMessages)
        {
            DialogId = dialogId;
            EncryptMessages = encryptMessages;
            SignMessages = signMessages;
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

        public bool EncryptMessages
        {
            get
            {
                return _encryptMessages;
            }

            set
            {
                _encryptMessages = value;
            }
        }

        public bool SignMessages
        {
            get
            {
                return _signMessages;
            }

            set
            {
                _signMessages = value;
            }
        }
    }
}
