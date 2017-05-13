using EncryptMessangerClient.Commands;
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
        private long _messageId;
        private UserInfo _author;
        private string _text;
        private bool _isAltered;
        private DateTime _sendDate;

        private long _attachId;
        private bool _hasAttach = false;
        //private List<Attachment> _attaches = new List<Attachment>();
        private CommandWithParametr _loadFileCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Author
        {
            get
            {
                if (_author != null)
                {
                    return _author.Login;
                }
                else
                {
                    return "";
                }
            }
        }
        public UserInfo AuthorInfo
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Author"));
            }
        }

        //public void SetAuthor(UserInfo author)
        //{
        //    _author = author;
        //    //PropertyChanged.Invoke("Messages");
        //}
        public string Text
        {
            get
            {
                //if (Attachment == null)
                //{
                return _text;
                //}
                //else
                //{
                //    return Attachment.FileName;
                //}
            }
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

        public DateTime SendDate
        {
            get
            {
                return _sendDate;
            }

            set
            {
                _sendDate = value;
            }
        }

        //internal List<Attachment> Attaches
        //{
        //    get
        //    {
        //        return _attaches;
        //    }

        //    //set
        //    //{
        //    //    _attaches = value;
        //    //}
        //}

        public DialogMessage(UserInfo author, long messageId, string text, DateTime sendDate, bool isAltered, CommandWithParametr loadFileCommand)
        {
            _isAltered = isAltered;
            _text = text;
            _author = author;
            _sendDate = sendDate;
            _loadFileCommand = loadFileCommand;
            _messageId = messageId;
        }
        //public DialogMessage( string text, bool isAltered, DateTime sendDate)
        //{
        //    _isAltered = isAltered;
        //    _text = text;
        //    SendDate = sendDate;         
        //}
        //public void AddAttachment(Attachment attachment)
        //{
        //    Attaches.Add(attachment);
        //}
        //public void RemoveAttachmentAt(int index)
        //{
        //    Attaches.RemoveAt(index);
        //}
        public bool Equals(DialogMessage other)
        {
            return this._messageId == other.MessageId; //other.AuthorInfo.Id == this.AuthorInfo.Id && other.Text.Equals(this.Text) && (other._isAltered == _isAltered);
        }
        public void AddAttachment(long attachmentId)
        {
            _hasAttach = true;
            _attachId = attachmentId;
        }
        //public Attachment Attachment
        //{
        //    get
        //    {
        //        return new Attachment();
        //    }
        //}
        public System.Windows.Visibility AttachButtonVisibility
        {
            get
            {
                if (_hasAttach)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }
        }
        public LoadFileCommandParams LoadFileParams
        {
            get
            {
                return new LoadFileCommandParams(AttachId, Text);
            }
        }
        public long AttachId
        {
            get
            {
                return _attachId;
            }
        }
        public CommandWithParametr LoadFileCommand
        {
            get
            {
                return _loadFileCommand;
            }
            set
            {
                if(value != null)
                {
                    _loadFileCommand = value;
                }
                
            }
        }

        public long MessageId
        {
            get
            {
                return _messageId;
            }

            set
            {
                _messageId = value;
            }
        }
        //private void Load(object param)
        //{
        //    LoadFileCommandParams p = param as LoadFileCommandParams;
        //}
    }
}
