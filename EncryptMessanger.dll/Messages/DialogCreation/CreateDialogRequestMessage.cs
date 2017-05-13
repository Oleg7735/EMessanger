using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages.DialogCreation
{
    public class CreateDialogRequestMessage:Message
    {
        private void Init()
        {
            _type = MessageType.CreateDialogRequestMessage;
        }
        public CreateDialogRequestMessage()
        {
            Init();
        }
        public CreateDialogRequestMessage(long creatorId, long[] membersId, string dialogName)
        {
            Init();
            CreatorId = creatorId;
            DialogName = dialogName;
            foreach(long l in membersId)
            {
                AddAtribute(new MessageAtribute(Atribute.MemberId, BitConverter.GetBytes(l)));
            }
            
        }
        public long CreatorId
        {
            get
            {
                return BitConverter.ToInt64(GetAttribute(Atribute.CreatorId), 0);
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.CreatorId, BitConverter.GetBytes(value)));
            }
        }
        public long[] MembersId
        {
            get
            {
                List<long> membersId = new List<long>();
                foreach(MessageAtribute atribute in _atributes)
                {
                    if(atribute.Name == Atribute.MemberId)
                    {
                        membersId.Add(BitConverter.ToInt64(atribute.Value, 0));
                    }
                }
                return membersId.ToArray();
            }
            
        }
        public string DialogName
        {
            get
            {
                return Encoding.UTF8.GetString(GetAttribute(Atribute.DialogName));
            }
            set
            {
                SetAtributeValue(new MessageAtribute(Atribute.DialogName, Encoding.UTF8.GetBytes(value)));
            }
        }
    }
}
