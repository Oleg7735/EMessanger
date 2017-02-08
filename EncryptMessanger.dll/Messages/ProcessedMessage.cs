using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class ProcessedMessage:Message
    {
        ProcessedMessage()
        {
            _type = MessageType.ProcessedMessage;
            _tag = "processed";
        }
    }
}
