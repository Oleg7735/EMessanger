using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Commands
{
    class LoadFileCommandParams
    {
        private long _attachId;
        private string _attachName;

        public LoadFileCommandParams(long attachId, string attachName)
        {
            _attachId = attachId;
            _attachName = attachName;
        }
        public long AttachId
        {
            get
            {
                return _attachId;
            }

            set
            {
                _attachId = value;
            }
        }

        public string AttachName
        {
            get
            {
                return _attachName;
            }

            set
            {
                _attachName = value;
            }
        }
    }
}
