using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ClientExitMessage:Message
    {
        public ClientExitMessage()
        {
            _type = MessageType.ClientExitMessage;
        }
        public ClientExitMessage(long userId)
        {
            _type = MessageType.ClientExitMessage;
            _atributes.Add(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
        }
        public long UserId
        {
            get { return BitConverter.ToInt64(GetAttribute(Atribute.UserId), 0); }
        }


    }
}
