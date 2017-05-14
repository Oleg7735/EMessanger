using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class MessageDeletedEventArgs:EventArgs
    {
        private long _messageId;
        public MessageDeletedEventArgs(long messageId)
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
