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
        public LoadDialogSessionEventArgs(long dialogId)
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
