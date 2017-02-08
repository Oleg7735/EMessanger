using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ResendibleMessage:Message
    {
        public ResendibleMessage()
        {
            _atributes.Add(new MessageAtribute(Atribute.To, new byte[] { 0}));
            _atributes.Add(new MessageAtribute(Atribute.From, new byte[] { 0 }));
        }
        public string To
        {
            set { setAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(value))); }
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.To)); }
        }
        public string From
        {
            set { setAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(value))); }
            get { return Encoding.UTF8.GetString(GetAttribute(Atribute.From)); }
        }
    }
}
