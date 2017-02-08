using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class AuthMessage:Message
    {
        public AuthMessage()
        {
            _tag = "auth";
            _type = MessageType.AuthMessage;
        }
    }
}
