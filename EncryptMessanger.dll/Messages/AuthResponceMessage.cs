using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class AuthResponceMessage:Message
    {
        public AuthResponceMessage(string ligin, byte[] password)
        {
            Init();
            _atributes.Add(new MessageAtribute(Atribute.Login, Encoding.UTF8.GetBytes(ligin)));
            _atributes.Add(new MessageAtribute(Atribute.Password, password));

        }
        public AuthResponceMessage()
        {
            Init();
        }
        private void Init()
        {
            _tag = "authresponce";
            _type = MessageType.AuthResponceMessage;
        }
        public string Login
        {
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.Login)); }
            //set { }
        }
        public byte[] Password
        {
            get { return GetAttribute(Atribute.Password); }
        }
    }
}
