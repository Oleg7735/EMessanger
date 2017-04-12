using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class DialogEncryptionSettingsMessage : ResendibleMessage
    {
        public DialogEncryptionSettingsMessage()
        {
            _type = MessageType.DialogEncryptionSettingsMessage;
        }
        public DialogEncryptionSettingsMessage(long dialogId, long currentUserId, bool useSign, bool useEncryption)
        {
            _type = MessageType.DialogEncryptionSettingsMessage;
            _atributes.Add(new MessageAtribute(Atribute.UseEncryption, BitConverter.GetBytes(useEncryption)));
            _atributes.Add(new MessageAtribute(Atribute.UseSignature, BitConverter.GetBytes(useSign)));
            //_atributes.Add(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(to)));
            //_atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            Dialog = dialogId;
            From = currentUserId;
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
        public bool UseEncrypt
        {
            get { return BitConverter.ToBoolean(GetAttribute(Atribute.UseEncryption),0); }
        }
        public bool UseSign
        {
            get { return BitConverter.ToBoolean(GetAttribute(Atribute.UseSignature), 0); }
        }
    }
}
