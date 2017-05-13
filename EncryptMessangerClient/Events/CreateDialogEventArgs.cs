using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class CreateDialogEventArgs:EventArgs
    {
        private long _creatorId;
        private long[] _membersId;
        private string _dialogName;

        public CreateDialogEventArgs(long creator, long[] members, string dialogName)
        {
            CreatorId = creator;
            MembersId = members;
            DialogName = dialogName;
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
    }
}
