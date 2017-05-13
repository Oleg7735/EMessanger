using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ClientStatusOnlineEventArgs
    {
        private long _id;
        public long Id
        {
            get { return _id; }
        }
        public ClientStatusOnlineEventArgs(long id)
        {
            _id = id;
        }
    }
}
