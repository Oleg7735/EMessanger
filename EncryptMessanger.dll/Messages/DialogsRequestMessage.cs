using EncryptMessanger.dll.SendibleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    //класс сообщения для запроса диалогов пользователя
    public class DialogsRequestMessage: Message
    {
        public DialogsRequestMessage( int dialogsCount, int dialogOffset)
        {
            _type = MessageType.DialogsRequestMessage;
            //AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
            AddAtribute(new MessageAtribute(Atribute.DialogsCount, BitConverter.GetBytes(dialogsCount)));
            AddAtribute(new MessageAtribute(Atribute.DialogOffset, BitConverter.GetBytes(dialogOffset)));

        }
        public DialogsRequestMessage()
        {
            _type = MessageType.DialogsRequestMessage;            
        }


        //public void SetUserId(long id)
        //{
        //    setAtributeValue(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(id)));
        //}
        public void SetDialogsCount(int count)
        {
            SetAtributeValue(new MessageAtribute(Atribute.DialogsCount, BitConverter.GetBytes(count)));
        }
        public void SetDialogsOffset(int offset)
        {
            SetAtributeValue(new MessageAtribute(Atribute.DialogOffset, BitConverter.GetBytes(offset)));
        }
        public int Offset
        {
            get
            {
                return BitConverter.ToInt32( GetAttribute(Atribute.DialogOffset), 0);
            }
        }
        public int Count
        {
            get
            {
                return BitConverter.ToInt32(GetAttribute(Atribute.DialogsCount), 0);
            }
        }
    }
}
