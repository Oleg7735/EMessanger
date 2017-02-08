using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ClientAKeyMessage:ResendibleMessage
    {
        public ClientAKeyMessage()
        {
            _type = MessageType.PublicKeyMessage;
        }
        //public string To
        //{
        //    set { setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(value))); }
        //    get { return Encoding.UTF8.GetString(GetAttribute(Atribute.To)); }
        //}
        //public string From
        //{
        //    set { setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(value))); }
        //    get { return Encoding.UTF8.GetString(GetAttribute(Atribute.From)); }
        //}
        public string RsaKey
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Key));
            }
        }
        public ClientAKeyMessage(string key, string to, string from)
        {
            _type = MessageType.ClientPublicKeyMessage;
            _atributes.Add(new MessageAtribute(Atribute.Key, Encoding.UTF8.GetBytes(key)));
            setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            //_atributes.Add(new MessageAtribute("key",key));
        }

    }
}
