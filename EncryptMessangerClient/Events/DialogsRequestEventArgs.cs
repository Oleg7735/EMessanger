using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class DialogsRequestEventArgs:EventArgs
    {
        //private long _userId;

        public DialogsRequestEventArgs(int dialogsCount, int dialogsOffset)
        {
            _dialogsCount = dialogsCount;
            _dialogsOffset = dialogsOffset;
        }
        private int _dialogsCount;
        private int _dialogsOffset;

        //public long UserId
        //{
        //    get
        //    {
        //        return _userId;
        //    }

        //    set
        //    {
        //        _userId = value;
        //    }
        //}

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

        public int DialogsOffset
        {
            get
            {
                return _dialogsOffset;
            }

            set
            {
                _dialogsOffset = value;
            }
        }
    }
}
