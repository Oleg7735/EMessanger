using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class DialogSessionSuccessEventArgs
    {
        private long _dialogId;

        public DialogSessionSuccessEventArgs(long dialogId)
        {
            DialogId = dialogId;
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
    }
}
