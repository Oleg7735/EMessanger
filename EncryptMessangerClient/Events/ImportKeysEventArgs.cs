using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class ImportKeysEventArgs:EventArgs
    {
        private long _dialogId;
        private string _fileName;

        public ImportKeysEventArgs(long dialogId, string fileName)
        {
            _dialogId = dialogId;
            _fileName = fileName;
        }
        public long DialogId
        {
            get
            {
                return _dialogId;
            }

            set
            {
                _dialogId = value;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
            }
        }
    }
}
