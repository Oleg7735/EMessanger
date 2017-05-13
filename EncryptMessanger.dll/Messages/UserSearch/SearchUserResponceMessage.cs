using EncryptMessanger.dll.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.UserSearch
{
    public class SearchUserResponceMessage:Message
    {
        private void Init()
        {
            _type = MessageType.SearchUserResponceMessage;
        }
        public SearchUserResponceMessage()
        {
            Init();
        }
        public SearchUserResponceMessage(long userId, string userLogin, UserState userState)
        {
            Init();
            Login = userLogin;
            State = userState;
            UserId = userId;
        }
        public string Login
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Login));
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Login, Encoding.UTF8.GetBytes(value)));
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
    }
}
