using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public CreateCryptoSessionRequest(long dialogId, long from, byte[] ipToConnect, int portToConnect)
        {
            _type = MessageType.CreateCryptoSessionRequest;
            Dialog = dialogId;
            From = from;
            SetAtributeValue(new MessageAtribute(Atribute.IP, ipToConnect));
            Port = portToConnect;
        }
        public CreateCryptoSessionRequest()
        {
            _type = MessageType.CreateCryptoSessionRequest;
            
        }
        public IPAddress Ip
        {
            get
            {
                return new IPAddress(GetAttribute(Atribute.IP));
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.IP, value.GetAddressBytes()));
            }
        }
        public int Port
        {
            get
            {
                return BitConverter.ToInt32(GetAttribute(Atribute.Port), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Port, BitConverter.GetBytes(value)));
            }
        }
    }
}
