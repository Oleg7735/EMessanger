using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class DeleteMessagesRequestMessage:Message
    {
        private void Init()
        { 
            _type = MessageType.DeleteMessagesRequestMessage;
        }
        public DeleteMessagesRequestMessage()
        {
            Init();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialogId"></param>
        /// <param name="userId"></param>
        /// <param name="offset">смещение для удаления сообщений(начиная с последних)</param>
        /// <param name="count">количество удаляемых сообщений (-1 - все соообщения с заданного смещения)</param>
        public DeleteMessagesRequestMessage(long dialogId, long userId)//, long offset, long count)
        {
            Init();
            DialogId = dialogId;
            UserId = userId;
            //Offset = offset;
            //Count = count;
        }
        public long DialogId
        {
            get
            { 
                return BitConverter.ToInt64(GetAttribute(Atribute.DialogId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.DialogId, BitConverter.GetBytes(value)));
            }
        }
        public long UserId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.UserId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.UserId, BitConverter.GetBytes(value)));
            }
        }
        //public long Offset
        //{
        //    get
        //    {
        //        return BitConverter.ToInt64(GetAttribute(Atribute.Offset), 0);
        //    }
        //    set
        //    {
        //        SetAtributeValue(new MessageAtribute(Atribute.Offset, BitConverter.GetBytes(value)));
        //    }
        //}
        //public long Count
        //{
        //    get
        //    {
        //        return BitConverter.ToInt64(GetAttribute(Atribute.Count), 0);
        //    }
        //    set
        //    {
        //        SetAtributeValue(new MessageAtribute(Atribute.Count, BitConverter.GetBytes(value)));
        //    }
        //}
    }
}
