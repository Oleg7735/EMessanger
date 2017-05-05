﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.FileMessages
{
    public class ReceiveFileRequest:Message
    {
        private void Init()
        {
            _type = MessageType.ReceiveFileRequest;
        }
        public ReceiveFileRequest(byte[] ip, int port)
        {
            Init();
            SetAtributeValue(new MessageAtribute(Atribute.IP, ip));
            Port = port;
        }
        public ReceiveFileRequest()
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
    }
}
