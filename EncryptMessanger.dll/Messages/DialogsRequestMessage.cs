using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    class DialogsRequestMessage: Message
    {
        public DialogsRequestMessage()
        {
            _type = MessageType.DialogsRequestMessage;

        }

        public void AddDialogInfo()
        {
            
        }
    }
}
