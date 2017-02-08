using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class EndStreamMessage:Message
    {
        private void Init()
        {
            _tag = "stream";
            _type = MessageType.EndStreamMessage;
        }
        public EndStreamMessage()
        {
            Init();
        }
        
    }
}
