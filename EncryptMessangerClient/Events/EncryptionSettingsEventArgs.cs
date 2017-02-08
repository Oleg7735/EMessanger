using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class EncryptionSettingsEventArgs
    {
        bool _sign;
        bool _encrypt;
        string _dialog;
        public string Dialog
        {
            get { return _dialog; }
        }
        public bool Sign
        {
            get { return _sign; }
        }
        public bool Encrypt
        {
            get { return _encrypt; }
        }
        public EncryptionSettingsEventArgs(bool sign, bool encrypt)
        {
            _sign = sign;
            _encrypt = encrypt;
        }
        public EncryptionSettingsEventArgs(bool sign, bool encrypt, string dialog)
        {
            _sign = sign;
            _encrypt = encrypt;
            _dialog = dialog;
        }
    }
}
