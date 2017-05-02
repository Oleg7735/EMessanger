using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.FileMessages
{
    class FileFragmentMessage:Message
    {
        private void Init()
        {
            _type = MessageType.FileFragmentMessage;
        }
        public FileFragmentMessage()
        {
            Init();
        }
        public FileFragmentMessage(byte[] data)
        {
            Init();
            SetAtributeValue(new MessageAtribute(Atribute.BinaryData, data));

        }
        public byte[] Data
        {
            get
            {
                return GetAttribute(Atribute.BinaryData);
            }
        }
    }
}
