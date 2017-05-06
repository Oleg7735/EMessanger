using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class LoadFileEventArgs:EventArgs
    {
        private long _dialogId;
        private long _attachmentId;
        private string _fileName;
        public LoadFileEventArgs(long attachId, long dialogId, string fileName)
        {
            AttachmentId = attachId;
            FileName = fileName;
            DialogId = dialogId;
        }

        public long AttachmentId
        {
            get
            {
                return _attachmentId;
            }

            set
            {
                _attachmentId = value;
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
    }
}
