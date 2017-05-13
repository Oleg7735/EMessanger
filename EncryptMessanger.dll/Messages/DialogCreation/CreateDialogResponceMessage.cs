using EncryptMessanger.dll.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.DialogCreation
{
    public class CreateDialogResponceMessage:Message
    {
        private void Init()
        {
            _type = MessageType.CreateDialogResponceMessage;
        }
        public CreateDialogResponceMessage()
        {
            Init();
        }
        public CreateDialogResponceMessage(ResponceState state, long dialogId, string error = "")
        {
            Init();
            State = state;
            Error = error;
            DialogId = dialogId;
        }
        public ResponceState State
        {
            get
            {
                return (ResponceState)BitConverter.ToInt32(GetAttribute(Atribute.ResponceState), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.ResponceState, BitConverter.GetBytes((int)value)));
            }
        }
        public long DialogId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.DialogId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(value)));
            }
        }
        public string Error
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Error));
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Error, Encoding.UTF8.GetBytes(value)));
            }

        }
    }
}
