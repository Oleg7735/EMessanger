using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ClientExitMessage:Message
    {
        public ClientExitMessage()
        {
            _type = MessageType.ClientExitMessage;
        }
        public ClientExitMessage(string userLogin)
        {
            _type = MessageType.ClientExitMessage;
            _atributes.Add(new MessageAtribute(Atribute.Clients, Encoding.UTF8.GetBytes(userLogin)));
        }
        public string User
        {
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.Clients)); }
        }


    }
}
