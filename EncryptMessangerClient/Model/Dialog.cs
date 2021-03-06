﻿using System;
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
        private long[] _membersId;
        private long _dialogId;
        private bool _sign = true;
        private bool _encrypt = true;
        private string _dialogName;
        private string _sessionErrorMessage;
        private bool _sessionError;
        private long _creatorId;
        //public bool _showError = false;
        private ObservableCollection<DialogMessage> _dialogMessages = new ObservableCollection<DialogMessage>();
        public ObservableCollection<DialogMessage> DialogMessages
        {
            get { return _dialogMessages; }
        }
        public string Name
        {
            get { return _dialogName; }
            private set
            {
                if(!_dialogName.Equals(value) && value != null)
                {
                    _dialogName = value;
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

        public long[] MembersId
        {
            get
            {
                return _membersId;
            }

            set
            {
                _membersId = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(Dialog other)
        {
            return other.DialogId == DialogId;
        }
        public Dialog()
        {

        }
        public Dialog(string name)
        {
            _dialogName = name;
        }
        public Dialog(string name, long dialogId, bool encrypt, bool sign, long creatorId, long[] membersId)
        {
            _dialogName = name;
            DialogId = dialogId;
            _encrypt = encrypt;
            _sign = sign;
            _creatorId = creatorId;
            MembersId = membersId;
        }

        public void BindMessagesToAuthor(UserInfo newUserInfo)
        {
            var q = _dialogMessages.Where(m => m.AuthorInfo.Id == newUserInfo.Id);
            foreach (DialogMessage message in q)
            {
                message.AuthorInfo = newUserInfo;
            }
        }
        public string SessionErrorMessage
        {
            get
            {
                if (String.IsNullOrEmpty(_sessionErrorMessage))
                    return "";
                else
                {
                    return _sessionErrorMessage;
                }
            }
            set
            {
                if(!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
                {
                    _sessionErrorMessage = value;
                    //_showError = 
                    OnPropertyChanged();
                }
                else
                {
                    _sessionErrorMessage = "";
                    OnPropertyChanged();
                }
            }
        }

        public bool SessionError
        {
            get
            {
                return _sessionError;
            }

            set
            {
                _sessionError = value;                
                OnPropertyChanged();
            }
        }

        public long CreatorId
        {
            get
            {
                return _creatorId;
            }

            set
            {
                _creatorId = value;
            }
        }

        public void AddSessionErrorMessage(string error)
        {
            SessionErrorMessage = error;
            SessionError = true;
        }
        public void ClearDialogSessionError()
        {
            SessionErrorMessage = "";
            SessionError = false;
        }
        public void SortMessages()
        {
            _dialogMessages = new ObservableCollection<DialogMessage>(DialogMessages.OrderBy(m => m.SendDate));
        }
    }
}
