using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.Events
{
    class ScrollMessagesEventArgs:EventArgs
    {
        private int _itemIndex;
        public ScrollMessagesEventArgs(int itemIndex)
        {
            _itemIndex = itemIndex;
        }

        public int ItemIndex
        {
            get
            {
                return _itemIndex;
            }

            set
            {
                _itemIndex = value;
            }
        }
    }
}
