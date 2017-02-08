using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class AKeyMessage : Message
    {

        private void Init()
        {
            _type = MessageType.PublicKeyMessage;
            _tag = "PublicKey";
        }
        public AKeyMessage(byte[] key)
        {
            Init();
            _atributes.Add(new MessageAtribute(Atribute.Key, key));
            //_atributes.Add(new MessageAtribute("key",key));
        }
        public AKeyMessage(string key)
        {
            Init();
            _atributes.Add(new MessageAtribute(Atribute.Key, Encoding.UTF8.GetBytes(key)));
            //_atributes.Add(new MessageAtribute("key",key));
        }
        public AKeyMessage()
        {
            Init();
            //SetAtributesFromeByte(byteMessage);
            //string key = Encoding.UTF8.GetString(byteMessage);
            //_atributes.Add(new MessageAtribute(Atribute.Key, key));
        }
        //@override
        public string RsaKey
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Key));
            }
        }
        public byte[] StartAtributeToByte()
        {
            string s;
            s = "<" + Tag + ">";
            s += Atributes[0].Value;
            s += "</" + Tag + ">";
            return Encoding.UTF8.GetBytes(s);

        }
    }
}
