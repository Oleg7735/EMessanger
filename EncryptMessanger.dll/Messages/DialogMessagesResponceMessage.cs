using EncryptMessanger.dll.SendibleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class DialogMessagesResponceMessage:Message
    {
        private void Init()
        {
            _type = MessageType.DialogMessagesResponceMessage;
        }
        public DialogMessagesResponceMessage()
        {
            Init();
        }
        public DialogMessagesResponceMessage(MessageSendibleInfo[] messageInfo)
        {
            Init();
            foreach (MessageSendibleInfo info in messageInfo)
            {
                AddDialogInfo(info);
            }
        }
        public void AddDialogInfo(MessageSendibleInfo messageInfo)
        {
            AddAtribute(new MessageAtribute(Atribute.MessageInfo, messageInfo.ToByte()));
        }
        public List<MessageSendibleInfo> GetMessagesInfo()
        {
            List<MessageSendibleInfo> messagesInfo = new List<MessageSendibleInfo>();

            foreach (MessageAtribute atribute in _atributes)
            {
                if (atribute.Name == Atribute.MessageInfo)
                {
                    messagesInfo.Add(new MessageSendibleInfo(atribute.Value));
                }
            }
            return messagesInfo;
        }
    }
}
