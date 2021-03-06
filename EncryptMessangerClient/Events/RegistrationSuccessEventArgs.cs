﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class RegistrationSuccessEventArgs
    {
        //Логин, под которым пользователь идентифицирован на сервере
        private string _login;
        private long _id;

        public string Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
            }
        }

        public long Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public RegistrationSuccessEventArgs(string login, long id)
        {
            Login = login;
            Id = id;
        }
    }
}
