using EncryptMessangerClient.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.ViewModel
{
    class EncryptionSettingsViewModel : INotifyPropertyChanged
    {
        private bool _sign = true;
        private bool _encrypt = true;
        public EventHandler<EncryptionSettingsEventArgs> SignSettingChanged;
        public EventHandler<EncryptionSettingsEventArgs> EncryptSettingChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool Sign
        {
            get { return _sign; }
            set
            {
                if(value!=_sign)
                {
                    _sign = value;
                    OnPropertyChanged();
                    SignSettingChanged?.Invoke(this, new EncryptionSettingsEventArgs(_sign, _encrypt));
                }
            }
        }
        public bool Encrypt
        {
            get { return _encrypt; }
            set
            {
                if (value != _encrypt)
                {
                    _encrypt = value;
                    OnPropertyChanged();
                    EncryptSettingChanged?.Invoke(this, new EncryptionSettingsEventArgs(_sign, _encrypt));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
