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
            _atributes.Add(new MessageAtribute(Atribute.DialogId, new byte[] { 0 }));
            _atributes.Add(new MessageAtribute(Atribute.From, new byte[] { 0 }));
        }
        public long Dialog
        {
            set
            {
                //SetAtributeValue(new MessageAtribute(Atribute.To, Encoding.UTF8.GetBytes(value)));
                SetAtributeValue(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(value)));
            }
            get
            {
                //return Encoding.UTF8.GetString(GetAttribute(Atribute.To));
                return BitConverter.ToInt64(GetAttribute(Atribute.DialogId),0);
            }
        }
        public long From
        {
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.From, BitConverter.GetBytes(value)));
                //SetAtributeValue(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(value)));
            }
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.From), 0);
            }
        }
    }
}
