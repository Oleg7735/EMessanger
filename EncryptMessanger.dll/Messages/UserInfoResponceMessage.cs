using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class UserInfoResponceMessage:Message
    {
        private void Init()
        {
            _type = MessageType.UserInfoResponceMessage;
        }
        public UserInfoResponceMessage()
        {
            Init();
        }
        public UserInfoResponceMessage(long userId, string login)
        {
            Init();
            AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
            AddAtribute(new MessageAtribute(Atribute.Login, Encoding.UTF8.GetBytes(login)));
        }
        public long UserId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.UserId), 0);
            }
        }
        public string Login
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Login));
            }
        }
    }
}
