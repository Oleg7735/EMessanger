using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ClientClientSignKeyMessage:ClientAKeyMessage
    {
        public ClientClientSignKeyMessage(string key, string to, string from)
        {
            _type = MessageType.ClientClientSignKeyMessage;

            _atributes.Add(new MessageAtribute(Atribute.Key, Encoding.UTF8.GetBytes(key)));
            setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            // _atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
           // _atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            //_atributes.Add(new MessageAtribute("key",key));
        }
        public ClientClientSignKeyMessage()
        {
            _type = MessageType.ClientClientSignKeyMessage;
        }
    }
}
