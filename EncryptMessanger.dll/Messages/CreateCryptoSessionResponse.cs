using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class CreateCryptoSessionResponse:ResendibleMessage
    {
        public long To
        {
            set { SetAtributeValue(new MessageAtribute(Atribute.To, BitConverter.GetBytes(value))); }
            get { return BitConverter.ToInt64(GetAttribute(Atribute.To), 0); }
        }
        //public string From
        //{
        //    set { setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(value))); }
        //    get { return Encoding.UTF8.GetString(GetAttribute(Atribute.From)); }
        //}
        public bool Response
        {
            set { SetAtributeValue(new MessageAtribute(Atribute.Response, BitConverter.GetBytes(value))); }
            get { return BitConverter.ToBoolean(GetAttribute(Atribute.Response),0); }
        }
        public CreateCryptoSessionResponse(long dialogId, long receiverId, long senderId, bool assept)
        {
            _type = MessageType.CreateCryptoSessionResponse;
            Dialog = dialogId;
            To = receiverId;
            From = senderId;
            Response = assept;
        }
        public CreateCryptoSessionResponse()
        {
            _type = MessageType.CreateCryptoSessionResponse;            
        }
    }
}
