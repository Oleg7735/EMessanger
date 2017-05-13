using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ClientOnlineMessage:Message
    {
        public ClientOnlineMessage()
        {
            _type = MessageType.ClientOnlineMessage;
            //_atributes.Add(new MessageAtribute(Atribute.Clients, ));
        }
        //public void AddClient(string login)
        //{
        //    login += "/";
        //    GetAttribute(Atribute.Clients);
        //}
        //public string[] Users
        //{
        //    get
        //    {
        //        return Encoding.UTF8.GetString(GetAttribute(Atribute.Clients)).Split('/');
        //    }
        //}
        public ClientOnlineMessage(long login)
        {

            _type = MessageType.ClientOnlineMessage;
            
            _atributes.Add(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(login)));
        }
        public long ClientId
        {
            get
            {
                return BitConverter.ToInt32(GetAttribute(Atribute.UserId), 0);

            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(value)));
            }
        }
        //public ClientOnlineMessage(string[] logins)
        //{

        //    _type = MessageType.ClientOnlineMessage;
        //    string joinedLogins = String.Join("/",logins);
        //    _atributes.Add(new MessageAtribute(Atribute.Clients, Encoding.UTF8.GetBytes(joinedLogins)));
        //}
    }
}
