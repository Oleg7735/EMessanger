using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ClientSKeyMessage:ResendibleMessage
    {
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
        public ClientSKeyMessage(byte[] initialVector, byte[] key, string to, string from)
        {
            _type = MessageType.ClientSymKeyMessage;

            _atributes.Add(new MessageAtribute(Atribute.IV, initialVector));
            _atributes.Add(new MessageAtribute(Atribute.Key, key));
            setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
        }
        public ClientSKeyMessage()
        {
            _type = MessageType.ClientSymKeyMessage;
        }

        public byte[] Key
        {
            get
            {
                return GetAttribute(Atribute.Key);
            }
        }
        public byte[] IV
        {
            get
            {
                return GetAttribute(Atribute.IV);
            }
        }
    }
}
