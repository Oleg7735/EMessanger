using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class RegistrationSuccessMessage:Message
    {
        public RegistrationSuccessMessage()
        {
            _type = MessageType.RegistrationSuccessMessage;
        }
    }
}
