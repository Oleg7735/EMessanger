using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class DeleteMessageEventArgs:EventArgs
    {
        private long _messageId;
        public DeleteMessageEventArgs(long messageId)
        {
            MessageId = messageId;
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
    }
}
