using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class UpdateDialogEncryptionKeysEventArgs
    {
        private long _dialogId;
        private long _userId;

        public UpdateDialogEncryptionKeysEventArgs(long dialogId, long userId)
        {
            _dialogId = dialogId;
            UserId = userId;
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

        public long UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
            }
        }
    }
}
