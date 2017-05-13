using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.SendibleData
{
    public class MessageSendibleInfo : ISendibleData
    {
        private byte[] _text;
        private byte[] _signature;
        private DateTime _sendTime;
        private long _authorId;
        private long _dialogId;
        private long _messageId;

        private bool _hasAttach = false;
        private long _attachId;
        //private byte[] _attachName;

        public MessageSendibleInfo()
        {

        }
        public MessageSendibleInfo(byte[] messageInfoBytes)
        {
            FillFromBytes(messageInfoBytes);
        }
        public MessageSendibleInfo(long messageId, long dialogId, long userId, DateTime sendTime, byte[] messageText)
        {
            DialogId = dialogId;
            AuthorId = userId;
            SendTime = sendTime;
            Text = messageText;
            MessageId = messageId;
            //Signature = signature;

        }
        //public void AddSignature(byte[] signature)
        //{
        //    Signature = signature;
        //}
        
        public byte[] Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
            }
        }

        public DateTime SendTime
        {
            get
            {
                return _sendTime;
            }

            set
            {
                _sendTime = value;
            }
        }

        public long AuthorId
        {
            get
            {
                return _authorId;
            }

            set
            {
                _authorId = value;
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

        public byte[] Signature
        {
            get
            {
                return _signature;
            }

            set
            {
                _signature = value;
            }
        }

        public bool HasAttach
        {
            get
            {
                return _hasAttach;
            }

            set
            {
                _hasAttach = value;
            }
        }

        public long AttachId
        {
            get
            {
                return _attachId;
            }

            set
            {
                _attachId = value;
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

        public void FillFromBytes(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);

            byte[] hasAttachBytes = new byte[1];
            ms.Read(hasAttachBytes, 0, 1);
            _hasAttach = BitConverter.ToBoolean(hasAttachBytes, 0);
            if(_hasAttach)
            {
                byte[] attachIdBytes = new byte[8];
                ms.Read(attachIdBytes, 0, 8);
                _attachId = BitConverter.ToInt64(attachIdBytes, 0);
            }
            byte[] messageIdBytes = new byte[8];
            ms.Read(messageIdBytes, 0, 8);

            byte[] dialogBytes = new byte[8];
            ms.Read(dialogBytes, 0, 8);

            byte[] userBytes = new byte[8];
            ms.Read(userBytes, 0, 8);

            byte[] dateBytes = new byte[8];
            ms.Read(dateBytes, 0, 8);

            byte[] textLengthBytes = new byte[4];
            ms.Read(textLengthBytes, 0, 4);

            int textLength = BitConverter.ToInt32(textLengthBytes, 0);

            _text = new byte[textLength];
            ms.Read(_text, 0, textLength);

            byte[] signatureLengthBytes = new byte[4];
            ms.Read(signatureLengthBytes, 0, 4);

            int signatureLength = BitConverter.ToInt32(signatureLengthBytes, 0);
            if (signatureLength != 0)
            {
                _signature = new byte[signatureLength];
                ms.Read(_signature, 0, signatureLength);
            }

            DialogId = BitConverter.ToInt64(dialogBytes, 0);
            AuthorId = BitConverter.ToInt64(userBytes, 0);
            SendTime = DateTime.FromBinary(BitConverter.ToInt64(dateBytes, 0));
            MessageId = BitConverter.ToInt64(messageIdBytes, 0);
        }
        public void AddAttach(long attachId)
        {
            _hasAttach = true;
            _attachId = attachId;
        }
        public byte[] ToByte()
        {
            
            long binaryData = _sendTime.ToBinary();
            byte[] hasAttachBytes = BitConverter.GetBytes(_hasAttach);
            byte[] textLengthBytes = BitConverter.GetBytes(_text.Length);
            byte[] signatureLengthBytes;
            if (Signature != null)
            {
                signatureLengthBytes = BitConverter.GetBytes(_signature.Length);
            }
            else
            {
                signatureLengthBytes = BitConverter.GetBytes(0);
            }
            byte[] messageIdBytes = BitConverter.GetBytes(_messageId);
            byte[] dateBytes = BitConverter.GetBytes(binaryData);
            byte[] userBytes = BitConverter.GetBytes(_authorId);
            byte[] dialogBytes = BitConverter.GetBytes(_dialogId);

            byte[] resultBytes;
            if(_hasAttach)
            {
                resultBytes = hasAttachBytes.Concat(BitConverter.GetBytes(_attachId)).ToArray();
                resultBytes = resultBytes.Concat(messageIdBytes).ToArray();
                //resultBytes = resultBytes.Concat(dialogBytes).ToArray();
            }
            else
            {
                resultBytes = hasAttachBytes.Concat(messageIdBytes).ToArray();
                //resultBytes = hasAttachBytes.Concat(dialogBytes).ToArray();
            }
            resultBytes = resultBytes.Concat(dialogBytes).ToArray();
            resultBytes = resultBytes.Concat(userBytes).ToArray();
            resultBytes = resultBytes.Concat(dateBytes).ToArray();
            resultBytes = resultBytes.Concat(textLengthBytes).ToArray();
            resultBytes = resultBytes.Concat(_text).ToArray();
            resultBytes = resultBytes.Concat(signatureLengthBytes).ToArray();
            if (Signature != null)
            {
                resultBytes = resultBytes.Concat(_signature).ToArray();
            }
            return resultBytes;

        }
    }
}
