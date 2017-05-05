using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.FileMessages
{
    public class SendFileRequest:Message
    {
        private void Init()
        {
            _type = MessageType.SendFileRequest;
        }
        public SendFileRequest(byte[] ip, int port, long senderId, long dialogId, byte[] name)
        {
            Init();
            SetAtributeValue(new MessageAtribute(Atribute.IP, ip));
            Port = port;
            Dialog = dialogId;
            SenderId = senderId;
            Name = name;
        }
        public SendFileRequest()
        {
            Init();
        }
        public IPAddress Ip
        {
            get
            {
                return new IPAddress(GetAttribute(Atribute.IP));
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.IP, value.GetAddressBytes()));
            }
        }
        public int Port
        {
            get
            {
                return BitConverter.ToInt32(GetAttribute(Atribute.Port), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Port, BitConverter.GetBytes(value)));
            }
        }
        public long SenderId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.UserId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(value)));
            }
        }
        public long Dialog
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.DialogId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(value)));
            }
        }
        public byte[] Name
        {
            get
            {
                return GetAttribute(Atribute.Name);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.Name, value));
            }
        }
    }
}
