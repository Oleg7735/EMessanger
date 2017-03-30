using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class RegistrationErrorMessage : Message
    {
        public RegistrationErrorMessage(string errorText)
        {
            setAtributeValue(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(errorText)));
            _type = MessageType.RegistrationErrorMessage;
        }
        public RegistrationErrorMessage()
        {
            //setAtributeValue(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(errorText)));
        }
        public byte[] byteText
        {
            set { setAtributeValue(new MessageAtribute(Atribute.Text, value)); }
            get { return GetAttribute(Atribute.Text); }
        }
        public string Text
        {
            set { setAtributeValue(new MessageAtribute(Atribute.Text, Encoding.UTF8.GetBytes(value))); }
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.Text)); }
        }
    }
}
