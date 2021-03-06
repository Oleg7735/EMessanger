﻿using System;
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
            Init();
        }
        public RegistrationSuccessMessage(long userId)
        {
            Init();
            AddAtribute(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(userId)));
        }


        private void Init()
        {
            _type = MessageType.RegistrationSuccessMessage;
        }

        public long UserId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.UserId), 0);
            }
        }
    }
}
