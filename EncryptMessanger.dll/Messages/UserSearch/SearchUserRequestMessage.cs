using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.UserSearch
{
    public class SearchUserRequestMessage:Message
    {
        private void Init()
        {
            _type = MessageType.UserSearchRequestMessage;
        }
        public SearchUserRequestMessage()
        {
            Init();
        }
        public SearchUserRequestMessage(string login, int offcet, int count)
        {
            Init();
            Login = login;
            Offcet = offcet;
            Count = count;
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
        public int Offcet
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
    }
}
