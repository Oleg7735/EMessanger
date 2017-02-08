using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class SKeyMessage : Message
    {
        public SKeyMessage(byte[] initialVector, byte[] key)
        {

            Init();
            _atributes.Add(new MessageAtribute(Atribute.Key, key));
            _atributes.Add(new MessageAtribute(Atribute.IV, initialVector));

        }
        private void Init()
        {
            _type = MessageType.SymKeyMessage;
            _tag = "SymetricKey";
        }
        public SKeyMessage()
        {
            Init();
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
