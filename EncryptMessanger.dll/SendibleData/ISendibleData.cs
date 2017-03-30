using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll
{
    interface ISendibleData
    {
        byte[] ToByte();
        void FillFromBytes(byte[] bytes);
        
    }
}
