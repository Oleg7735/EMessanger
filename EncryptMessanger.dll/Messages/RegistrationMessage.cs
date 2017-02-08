using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class RegistrationMessage : Message
    {
        public RegistrationMessage()
        {
            _type = MessageType.RegistrationMessage;
        }
        public RegistrationMessage(string login, byte[] password)
        {
            _type = MessageType.RegistrationMessage;
            _atributes.Add(new MessageAtribute(Atribute.Login, Encoding.UTF8.GetBytes(login)));
            _atributes.Add(new MessageAtribute(Atribute.Password, password));
        }
        public byte[] ByteLogin
        {
            get
            {
                return GetAttribute(Atribute.Login);
            }
        }
        public byte[] BytePassword
        {
            get
            {
                return GetAttribute(Atribute.Password);
            }
        }
        public string Login
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Login));
            }
        }
        public string Password
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Password));
            }
        }
    }
}
