using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class StartStreamMessage : Message
    {
        private void Init()
        {
            _tag = "stream";
            _type = MessageType.StartStreamMessage;
        }
        public StartStreamMessage()
        {
            Init();
        }
        public StartStreamMessage(string from)
        {
            _atributes.Add(new MessageAtribute(Atribute.From, Encoding.UTF8.GetBytes(from)));
            Init();
        }
        //public long From
        //{
        //    get
        //    {
        //        return BitConverter.ToInt64(GetAttribute(Atribute.From),0);
        //    }
        //}
        public string From
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.From));
            }
        }
    }
}
