using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class SearchUserEventArgs:EventArgs
    {
        private string _userLogin;
        private int _count;
        private int _offset;

        public SearchUserEventArgs(string userLogin, int likeOffcet, int likeCount)
        {
            _userLogin = userLogin;
            _count = likeCount;
            _offset = likeOffcet;
        }
        public string UserLogin
        {
            get
            {
                return _userLogin;
            }

            set
            {
                _userLogin = value;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
            }
        }

        public int Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
            }
        }
    }
}
