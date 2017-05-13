using EncryptMessanger.dll.SendibleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.DialogCreation
{
    public class DialogCreatedMessage:Message
    {
        private void Init()
        {
            _type = MessageType.DialogCreatedMessage;
        }
        public DialogCreatedMessage()
        {
            Init();
        }
        public DialogCreatedMessage(DialogSendibleInfo dialogData)
        {
            Init();
            Info = dialogData;
        }
        public DialogSendibleInfo Info
        {
            get
            {
                return new DialogSendibleInfo(GetAttribute(Atribute.DialogInfo));
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.DialogInfo, value.ToByte()));
            }
        }
    }
}
