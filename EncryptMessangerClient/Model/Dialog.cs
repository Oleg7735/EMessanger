using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Model
{
    class Dialog : INotifyPropertyChanged, IEquatable<Dialog>
    {
        private long _dialogId;
        private bool _sign = true;
        private bool _encrypt = true;
        private string _login;
        private ObservableCollection<DialogMessage> _dialogMessages = new ObservableCollection<DialogMessage>();
        public ObservableCollection<DialogMessage> DialogMessages
        {
            get { return _dialogMessages; }
        }
        public string Login
        {
            get { return _login; }
            private set
            {
                if(!_login.Equals(value) && value != null)
                {
                    _login = value;
                }
            }
        }
        public bool Sign
        {
            get { return _sign; }
            set
            {
                if(value != _sign)
                {
                    _sign = value;
                }
            }
        }
        /// <summary>
        /// Свойство, указывающее, зашифрован ли данный диалог
        /// </summary>
        public bool Encrypt
        {
            get { return _encrypt; }
            set
            {
                if (value != _encrypt)
                {
                    _encrypt = value;
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(Dialog other)
        {
            return other.Login.Equals(_login);
        }
        public Dialog()
        {

        }
        public Dialog(string login)
        {
            _login = login;
        }
    }
}
