using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Model
{
    class DialogMessage : INotifyPropertyChanged, IEquatable<DialogMessage>
    {
        private string _alteredMessage = "Сообщение было изменено!";

        private UserInfo _author;
        private string _text;
        private bool _isAltered;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserInfo Author
        {
            get { return _author; }
        }
        public string Text
        {
            get { return _text; }
        }
        //public DialogMessage(string author, string text)
        //{
        //    _text = text;
        //    _author = author;
        //}
        public string AlteredErrorMessage
        {
            get
            {
                if (_isAltered)
                {
                    return _alteredMessage;
                }
                return "";
            }
        }
        public DialogMessage(UserInfo author, string text, bool isAltered)
        {
            _isAltered = isAltered;
            _text = text;
            _author = author;
        }

        public bool Equals(DialogMessage other)
        {
            return other.Author.Equals(this.Author) && other.Text.Equals(this.Text) && (other._isAltered == _isAltered);
        }
    }
}
