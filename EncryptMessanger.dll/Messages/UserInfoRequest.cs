using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class UserInfoRequestMessage:Message
    {

        private void Init()
        {
            _type = MessageType.UserInfoRequestMessage;
        }
        public UserInfoRequestMessage()
        {
            Init();
        }
        public UserInfoRequestMessage(long userId)
        {
            Init();
            AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
        }
        public long UserId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.UserId), 0);
            }
        }        

    }
}
