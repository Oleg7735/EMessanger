using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.FileMessages
{
    class EndFileMessage:Message
    {
        private void Init()
        {
            _type = MessageType.EndFileMessage;
        }
        public byte[] Signature
        {
            get
            {
                return GetAttribute(Atribute.Signature);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Signature, value));
            }
        }
        //public byte[] Name
        //{
        //    get
        //    {
        //        return GetAttribute(Atribute.Name);
        //    }
        //    set
        //    {
        //        SetAtributeValue(new MessageAtribute(Atribute.Name, value));
        //    }
        //}
        public EndFileMessage()
        {
            Init();
        }
        public EndFileMessage(byte[] signature)
        {
            Init();
           //Name = name;
            Signature = signature;
        }
    }
}
