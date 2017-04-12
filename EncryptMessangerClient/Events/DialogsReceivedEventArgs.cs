using EncryptMessanger.dll.SendibleData;
using EncryptMessangerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class DialogsReceivedEventArgs:EventArgs
    {
        public DialogsReceivedEventArgs(List<DialogSendibleInfo> dialogs)
        {
            Dialogs = dialogs;
        }
        private List<DialogSendibleInfo> _dialogs;

        public List<DialogSendibleInfo> Dialogs
        {
            get
            {
                return _dialogs;
            }

            set
            {
                _dialogs = value;
            }
        }
    }
}
