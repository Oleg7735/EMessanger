using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.SendibleData
{
     public class DialogSendibleInfo : ISendibleData
    {
        private long _dialogId;
        private string _dialogName;
        private DateTime _creationTime;
        private bool _encryptMessages;
        private bool _signMessages;
        private long[] _membersId;

        public DialogSendibleInfo(long dialogId, string dialogName, DateTime creationTime, bool encryptMessages, bool signMessages, long[] membersId)
        {
            _dialogId = dialogId;
            _dialogName = dialogName;
            _creationTime = creationTime;
            _encryptMessages = encryptMessages;
            _signMessages = signMessages;
            MembersId = membersId;
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

        public string DialogName
        {
            get
            {
                return _dialogName;
            }

            set
            {
                _dialogName = value;
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return _creationTime;
            }

            set
            {
                _creationTime = value;
            }
        }

        public bool EncryptMessages
        {
            get
            {
                return _encryptMessages;
            }

            set
            {
                _encryptMessages = value;
            }
        }

        public bool SignMessages
        {
            get
            {
                return _signMessages;
            }

            set
            {
                _signMessages = value;
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

        public DialogSendibleInfo()
        {

        }
        public DialogSendibleInfo(byte[] bytes)
        {
            this.FillFromBytes(bytes);
        }
        public void FillFromBytes(byte[] bytes)
        {
            //int nameByteLength = bytes.Length - 18;
            MemoryStream ms = new MemoryStream(bytes);
            byte[] idBytes = new byte[8];
            ms.Read(idBytes,0,8);
            byte[] creationTimeBytes = new byte[8];
            ms.Read(creationTimeBytes, 0, 8);
            byte[] encryptMessagesBytes = new byte[1];
            ms.Read(encryptMessagesBytes, 0, 1);
            byte[] signMessagesBytes = new byte[1];
            ms.Read(signMessagesBytes, 0, 1);
            byte[] membersIdLengthBytes = new byte[4];
            ms.Read(membersIdLengthBytes, 0, 4);
            int membersIdlength = BitConverter.ToInt32(membersIdLengthBytes, 0);
            byte[] membersIdBytes = new byte[membersIdlength];
            ms.Read(membersIdBytes, 0, membersIdlength);
            byte[] dialogNameLengthBytes = new byte[4];
            ms.Read(dialogNameLengthBytes, 0, 4);
            int dialogNamelength = BitConverter.ToInt32(dialogNameLengthBytes, 0);
            byte[] dialogNameBytes = new byte[dialogNamelength];
            ms.Read(dialogNameBytes, 0, dialogNamelength);

            DialogId = BitConverter.ToInt64(idBytes, 0);
            long binaryData = BitConverter.ToInt64(creationTimeBytes, 0);
            CreationTime = DateTime.FromBinary(binaryData);
            EncryptMessages = BitConverter.ToBoolean(encryptMessagesBytes, 0);
            SignMessages = BitConverter.ToBoolean(signMessagesBytes, 0);
            DialogName = Encoding.UTF8.GetString(dialogNameBytes);

            int offset = 0;
            _membersId = new long[membersIdBytes.Length/8];
            for(int i = 0; i < membersIdBytes.Length/8; i ++)
            {
                _membersId[i] = BitConverter.ToInt64(membersIdBytes, offset);
                offset += 8;
            }
        }

        public byte[] ToByte()
        {
            long binaryData = CreationTime.ToBinary();
            byte[] idBytes = BitConverter.GetBytes(DialogId);
            byte[] nameBytes = Encoding.UTF8.GetBytes(DialogName);
            byte[] nameLengthBytes = BitConverter.GetBytes(nameBytes.Length);
            byte[] timeBytes = BitConverter.GetBytes(binaryData);
            byte[] encryptMessagesBytes = BitConverter.GetBytes(EncryptMessages);
            byte[] signMessagesBytes = BitConverter.GetBytes(SignMessages);
            
            byte[] membersIdBytes = new byte[_membersId.Length * 8];

            byte[] buffer;
            
            int offset = 0;
            foreach(long memberId in _membersId)
            {
                buffer = BitConverter.GetBytes(memberId);
                buffer.CopyTo(membersIdBytes, offset);
                offset += 8;
            }
            byte[] membersIdLengthBytes = BitConverter.GetBytes(membersIdBytes.Length);

            byte[] result = idBytes.Concat(timeBytes).ToArray();
            result = result.Concat(encryptMessagesBytes).ToArray();
            result = result.Concat(signMessagesBytes).ToArray();
            result = result.Concat(membersIdLengthBytes).ToArray();
            result = result.Concat(membersIdBytes).ToArray();
            result = result.Concat(nameLengthBytes).ToArray();
            result = result.Concat(nameBytes).ToArray();
                        
            return result;
        }
    }
}
