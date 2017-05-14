using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.SingleMessageDeletion
{
    public class MessageDeletedMessage:Message
    {
        private void Init()
        {
            _type = MessageType.MessageDeletedMessage;
        }
        public MessageDeletedMessage()
        {
            Init();
        }
        public MessageDeletedMessage(long messageId)
        {
            Init();
            MessageId = messageId;
        }
        public long MessageId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.MessageId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.MessageId, BitConverter.GetBytes(value)));
            }
        }
    }
}
