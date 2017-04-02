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
        public DialogsRequestMessage(long userId, int dialogsCount)
        {
            _type = MessageType.DialogsRequestMessage;
            AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
            AddAtribute(new MessageAtribute(Atribute.DialogsCount, BitConverter.GetBytes(dialogsCount)));

        }
        public DialogsRequestMessage()
        {
            _type = MessageType.DialogsRequestMessage;
            
        }


        public void SetUserId(long id)
        {
            setAtributeValue(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(id)));
        }
        public void SetDialogsCount(int count)
        {
            setAtributeValue(new MessageAtribute(Atribute.DialogsCount, BitConverter.GetBytes(count)));
        }
    }
}
