using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class AuthSuccessMessage:Message
    {
        private void Init()
        {
            _type = MessageType.AuthSuccessMessage;
        }
        public long UserId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.UserId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(value)));
            }
        }
        public AuthSuccessMessage()
        {
            Init();
        }
        public AuthSuccessMessage(long userId)
        {
            AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
            Init();
        }

    }
}
