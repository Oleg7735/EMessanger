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

        public DialogUserInfoReceivedEventArgs()
        {

        }
        public DialogUserInfoReceivedEventArgs(long id, string login)
        {
            UserId = id;
            Login = login;
        }
    }
}
