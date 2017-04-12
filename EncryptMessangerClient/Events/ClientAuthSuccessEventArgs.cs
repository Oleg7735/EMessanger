using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ClientAuthSuccessEventArgs:EventArgs
    {
        private string _login;
        private long _id;

        public ClientAuthSuccessEventArgs(long id, string login)
        {
            _id = id;
            _login = login;
        }
        public ClientAuthSuccessEventArgs()
        {

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
    }
}
