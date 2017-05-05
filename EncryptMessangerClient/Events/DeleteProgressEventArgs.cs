using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class DeleteProgressEventArgs:EventArgs
    {
        FileSendProgress _progress;
        public DeleteProgressEventArgs(FileSendProgress progress)
        {
            _progress = progress;
        }

        internal FileSendProgress Progress
        {
            get
            {
                return _progress;
            }

            set
            {
                _progress = value;
            }
        }
    }
}
