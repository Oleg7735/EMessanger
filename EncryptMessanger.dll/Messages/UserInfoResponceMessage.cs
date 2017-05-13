using EncryptMessanger.dll.Enums;
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
        public UserInfoResponceMessage(long userId, string login, UserState state)
        {
            Init();
            AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
            AddAtribute(new MessageAtribute(Atribute.Login, Encoding.UTF8.GetBytes(login)));
            State = state;
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
        public UserState State
        {
            get
            {
                return (UserState)Enum.ToObject(typeof(UserState), BitConverter.ToInt32(GetAttribute(Atribute.UserState), 0)); 
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.UserState, BitConverter.GetBytes((int)value)));
            }
        }
    }
}
