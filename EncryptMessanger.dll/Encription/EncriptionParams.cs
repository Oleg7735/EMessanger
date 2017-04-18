using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Encription
{
    static class EncriptionParams
    {
        private const int _aesKeySize = 16;
        private const int _aesIvSize = 16;
        private const int _aesEncrKeySize = 128;
        private const int _aesEncrIVSize = 128;
        private const string _rsaEncryptionKeyMark = "Encryption";
        private const string _rsaSignKeyMark = "Sign";
        private const string _rsaVerifyKeyMark = "Verify";

        public static int AesKeySize
        {
            get
            {
                return _aesKeySize;
            }
        }

        public static int AesIvSize
        {
            get
            {
                return _aesIvSize;
            }
        }

        public static int AesEncrKeySize
        {
            get
            {
                return _aesEncrKeySize;
            }
        }

        public static int AesEncrIVSize
        {
            get
            {
                return _aesEncrIVSize;
            }
        }

        public static string RsaEncryptionKeyMark
        {
            get
            {
                return _rsaEncryptionKeyMark;
            }
        }

        public static string RsaSignKeyMark
        {
            get
            {
                return _rsaSignKeyMark;
            }
        }

        public static string RsaVerifyKeyMark
        {
            get
            {
                return _rsaVerifyKeyMark;
            }
        }
    }
}
