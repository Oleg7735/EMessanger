using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    class AuthErrorMessage:Message
    {
        public string Text
        {
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.Text)); }
        }
        public AuthErrorMessage(string errorText)
        {
            _type = MessageType.AuthErrorMessage;
            Atributes.Add(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(errorText)));
        }
        public AuthErrorMessage()
        {
            _type = MessageType.AuthErrorMessage;
        }
    }
}
