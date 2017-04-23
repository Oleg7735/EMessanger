using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class DialogMessagesRequestMessage:Message
    {
        private void Init()
        {
            _type = MessageType.DialogMessagesRequestMessage;
        }
        public DialogMessagesRequestMessage(long dialogId, int messagesCount, int messagesOffset)
        {
            Init();
            //AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
            AddAtribute(new MessageAtribute(Atribute.Count, BitConverter.GetBytes(messagesCount)));
            AddAtribute(new MessageAtribute(Atribute.Offset, BitConverter.GetBytes(messagesOffset)));
            AddAtribute(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(dialogId)));

        }
        public DialogMessagesRequestMessage()
        {
            Init();
        }


        //public void SetUserId(long id)
        //{
        //    setAtributeValue(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(id)));
        //}
        //public void SetMessagesCount(int count)
        //{
        //    SetAtributeValue(new MessageAtribute(Atribute.Count, BitConverter.GetBytes(count)));
        //}
        //public void SetMessagesOffset(int offset)
        //{
        //    SetAtributeValue(new MessageAtribute(Atribute.Offset, BitConverter.GetBytes(offset)));
        //}
        
        public int Offset
        {
            get
            {
                return BitConverter.ToInt32(GetAttribute(Atribute.Offset), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Offset, BitConverter.GetBytes(value)));

            }
        }
        public int Count
        {
            get
            {
                return BitConverter.ToInt32(GetAttribute(Atribute.Count), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Count, BitConverter.GetBytes(value)));
            }
        }
        public long dialogId
        {
            get
            {
                return BitConverter.ToInt32(GetAttribute(Atribute.DialogId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(value)));

            }
        }
    }
}
