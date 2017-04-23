using EncryptMessanger.dll.SendibleData;
using EncryptMessangerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    public class MessagesReceivedEventArgs:EventArgs
    {
        private List<DialogMessage> _messages = new List<DialogMessage>();
        private long _dialog;

        public long Dialog
        {
            get
            {
                return _dialog;
            }

            set
            {
                _dialog = value;
            }
        }

        //private long _dialogId;
        //private long _authorId;
        //private DateTime _date;
        //private string _text;

        //public long DialogId
        //{
        //    get
        //    {
        //        return _dialogId;
        //    }

        //    set
        //    {
        //        _dialogId = value;
        //    }
        //}

        //public long AuthorId
        //{
        //    get
        //    {
        //        return _authorId;
        //    }

        //    set
        //    {
        //        _authorId = value;
        //    }
        //}

        //public DateTime Date
        //{
        //    get
        //    {
        //        return _date;
        //    }

        //    set
        //    {
        //        _date = value;
        //    }
        //}

        //public string Text
        //{
        //    get
        //    {
        //        return _text;
        //    }

        //    set
        //    {
        //        _text = value;
        //    }
        //}

        public MessagesReceivedEventArgs(long dialogId)
        {
            Dialog = dialogId;
        }

        public void AddMessage(long authorId, string text, DateTime sendTime, bool isAltered)
        {
            _messages.Add(new DialogMessage(new UserInfo(authorId), text, sendTime, isAltered));
        }
        internal DialogMessage[] GetDialogMessages()
        {
            return _messages.ToArray();
        } 
        //public MessageSendibleInfo Messages
        //{
        //    get
        //    {
        //        return _messages;
        //    }

        //    set
        //    {
        //        _messages = value;
        //    }
        //}
    }
}
