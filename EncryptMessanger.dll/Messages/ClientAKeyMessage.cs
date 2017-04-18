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
        public ClientAKeyMessage(string key, long dialogId, long userId)
        {
            _type = MessageType.ClientPublicKeyMessage;
            AddAtribute(new MessageAtribute(Atribute.Key, Encoding.UTF8.GetBytes(key)));
            //AddAtribute(new MessageAtribute(Atribute.To, BitConverter.GetBytes(dialogId)));
            //AddAtribute(new MessageAtribute(Atribute.From, BitConverter.GetBytes(userId)));
            //_atributes.Add(new MessageAtribute(Atribute.Key, Encoding.UTF8.GetBytes(key)));
            SetAtributeValue(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(dialogId)));
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            SetAtributeValue(new MessageAtribute(Atribute.From, BitConverter.GetBytes(userId)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            //_atributes.Add(new MessageAtribute("key",key));
        }

    }
}
