using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class AuthSuccessMessage:Message
    {
        public AuthSuccessMessage()
        {
            _type = MessageType.AuthSuccessMessage;
        }
    }
}
