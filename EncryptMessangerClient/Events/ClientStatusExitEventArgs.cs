using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class ClientStatusExitEventArgs
    {
        private long _id;
        public long Id
        {
            get { return _id; }
        }
        public ClientStatusExitEventArgs(long id)
        {
            _id = id;
        }
    }
}
