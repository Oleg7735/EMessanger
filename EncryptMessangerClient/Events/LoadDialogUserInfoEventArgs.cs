using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class LoadDialogUserInfoEventArgs:EventArgs
    {
        private long _userId;

        public LoadDialogUserInfoEventArgs(long userId)
        {
            UserId = userId;
        }
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
    }
}
