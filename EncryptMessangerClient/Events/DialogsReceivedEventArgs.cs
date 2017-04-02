using EncryptMessanger.dll.SendibleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class DialogsReceivedEventArgs:EventArgs
    {
        public DialogsReceivedEventArgs(List<DialogSendibleInfo> dialogsInfo)
        {
            DialogsInfo = dialogsInfo;
        }
        private List<DialogSendibleInfo> _dialogsInfo;

        public List<DialogSendibleInfo> DialogsInfo
        {
            get
            {
                return _dialogsInfo;
            }

            set
            {
                _dialogsInfo = value;
            }
        }
    }
}
