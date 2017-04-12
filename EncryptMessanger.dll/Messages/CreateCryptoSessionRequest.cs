using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class CreateCryptoSessionRequest:ResendibleMessage
    {
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
        public CreateCryptoSessionRequest(long dialogId, long from)
        {
            _type = MessageType.CreateCryptoSessionRequest;
            Dialog = dialogId;
            From = from;
        }
        public CreateCryptoSessionRequest()
        {
            _type = MessageType.CreateCryptoSessionRequest;
            
        }
    }
}
