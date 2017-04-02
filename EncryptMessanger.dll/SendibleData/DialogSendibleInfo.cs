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

        public DialogSendibleInfo()
        {

        }
        public DialogSendibleInfo(byte[] bytes)
        {
            this.FillFromBytes(bytes);
        }
        public void FillFromBytes(byte[] bytes)
        {
            int nameByteLength = bytes.Length - 16;
            MemoryStream ms = new MemoryStream(bytes);
            byte[] idBytes = new byte[8];
            ms.Read(idBytes,0,8);
            byte[] creationTimeBytes = new byte[8];
            ms.Read(creationTimeBytes, 0, 8);
            byte[] dialogNameBytes = new byte[nameByteLength];
            ms.Read(dialogNameBytes, 0, nameByteLength);

            _dialogId = BitConverter.ToInt64(idBytes, 0);
            long binaryData = BitConverter.ToInt64(creationTimeBytes, 0);
            _creationTime = DateTime.FromBinary(binaryData);
            _dialogName = Encoding.UTF8.GetString(dialogNameBytes);
        }

        public byte[] ToByte()
        {
            long binaryData = _creationTime.ToBinary();
            byte[] idBytes = BitConverter.GetBytes(_dialogId);
            byte[] nameBytes = Encoding.UTF8.GetBytes(_dialogName);
            byte[] timeBytes = BitConverter.GetBytes(binaryData);
            
            return (byte[])idBytes.Concat(timeBytes.Concat(nameBytes) );
        }
    }
}
