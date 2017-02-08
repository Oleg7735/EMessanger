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
        public string[] Users
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.Clients)).Split('/');
            }
        }
        public ClientOnlineMessage(string login)
        {

            _type = MessageType.ClientOnlineMessage;
            
            _atributes.Add(new MessageAtribute(Atribute.Clients, Encoding.UTF8.GetBytes(login)));
        }
        public ClientOnlineMessage(string[] logins)
        {

            _type = MessageType.ClientOnlineMessage;
            string joinedLogins = String.Join("/",logins);
            _atributes.Add(new MessageAtribute(Atribute.Clients, Encoding.UTF8.GetBytes(joinedLogins)));
        }
    }
}
