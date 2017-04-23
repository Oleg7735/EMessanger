using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class DialogSessionFaildEventArgs: EventArgs
    {
        private long _dialogId;
        private string _errorMessage;

        public DialogSessionFaildEventArgs(long dialogId, string errorMessage)
        {
            DialogId = dialogId;
            ErrorMessage = errorMessage;
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

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
            }
        }
    }
}
