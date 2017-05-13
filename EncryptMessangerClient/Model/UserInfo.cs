using EncryptMessanger.dll.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Model
{
    class UserInfo
    {
        private string _login;
        private long _id;
        private UserState _state;

        public UserInfo()
        {

        }
        public UserInfo(long id)
        {
            _id = id;            
        }
        public UserInfo(long id, string login, UserState state)
        {
            _id = id;
            _login = login;
            _state = state;
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
        public string StringState
        {
            get
            {
                return _state.ToString();
            }
        }
    }
}
