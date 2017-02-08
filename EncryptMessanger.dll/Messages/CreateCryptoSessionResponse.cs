using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class CreateCryptoSessionResponse:ResendibleMessage
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
        public bool Response
        {
            set { setAtributeValue(new MessageAtribute(Atribute.Response, BitConverter.GetBytes(value))); }
            get { return BitConverter.ToBoolean(GetAttribute(Atribute.Response),0); }
        }
        public CreateCryptoSessionResponse(string to, string from, bool assept)
        {
            _type = MessageType.CreateCryptoSessionResponse;
            To = to;
            From = from;
            Response = assept;
        }
        public CreateCryptoSessionResponse()
        {
            _type = MessageType.CreateCryptoSessionResponse;            
        }
    }
}
