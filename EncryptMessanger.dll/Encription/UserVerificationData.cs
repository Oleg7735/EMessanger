using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Encription
{
    /// <summary>
    /// Инкапсулирует информацию о ключах для верификации ЭЦП.
    /// </summary>
    public class UserVerificationData
    {
        private long _userID;
        private RSACryptoServiceProvider _rsaToVerify;
        public UserVerificationData(long userId, RSACryptoServiceProvider rsaToVerify)
        {
            _userID = userId;
            _rsaToVerify = rsaToVerify;
        }
        
        public long UserID
        {
            get
            {
                return _userID;
            }

            set
            {
                _userID = value;
            }
        }

        public RSACryptoServiceProvider RsaToVerify
        {
            get
            {
                return _rsaToVerify;
            }

            set
            {
                _rsaToVerify = value;
            }
        }
    }
}
