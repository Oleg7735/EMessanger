using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class DialogsRequestEventArgs:EventArgs
    {
        private long _userId;
        private int _dialogsCount;

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

        public int DialogsCount
        {
            get
            {
                return _dialogsCount;
            }

            set
            {
                _dialogsCount = value;
            }
        }
    }
}
