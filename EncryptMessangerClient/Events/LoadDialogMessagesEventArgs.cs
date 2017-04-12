using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class LoadDialogMessagesEventArgs:EventArgs
    {
        private long _dialogId;
        private int _count;
        private int _offset;

        public LoadDialogMessagesEventArgs(long dialogId, int count, int offset)
        {
            DialogId = dialogId;
            Count = count;
            Offset = offset;
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

        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
            }
        }

        public int Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
            }
        }
    }
}
