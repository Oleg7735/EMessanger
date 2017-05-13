using EncryptMessanger.dll.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class DialogUserInfoReceivedEventArgs
    {
        private long _userId;
        private string _login;
        private UserState _state;

        public long UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
            }
        }

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

        public UserState State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }

        public DialogUserInfoReceivedEventArgs()
        {

        }
        public DialogUserInfoReceivedEventArgs(long id, string login, UserState state)
        {
            UserId = id;
            Login = login;
            State = state;

        }
    }
}
