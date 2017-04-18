using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using EncryptMessanger.dll.Messages;

namespace EncryptMessanger.dll.Encription
{
    //public enum EncryptionSettings
    //{
    //    Sign, Encrypt, SignAndEncrypt
    //};
    public class ClientClientEncryptedSession
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        public bool UseEncryption { get; set; }
        public bool UseSignature { get; set; }

        ICryptoTransform _encryptor;
        ICryptoTransform _decryptor;
        AesManaged _aes;

        RSACryptoServiceProvider _rsaToSign;
        RSACryptoServiceProvider _rsaToVerify;

        long _dialogId;
        public long Dialog
        {
            get { return _dialogId; }
        }

        

        public ClientClientEncryptedSession(AesManaged aes, long dialogId, RSACryptoServiceProvider rsaToSign, RSACryptoServiceProvider rsaToVerify)
        {
            _dialogId = dialogId;
            //_encryptor = encryptor;
            //_decryptor = decryptor;
            _encryptor = aes.CreateEncryptor();
            _decryptor = aes.CreateDecryptor();
            _aes = aes;

            _rsaToSign = rsaToSign;
            _rsaToVerify = rsaToVerify;
            UseEncryption = true;
            UseSignature = true;
        }
        public byte[] Dectypt(byte[] data)
        {
            if (UseEncryption)
            {
                return _decryptor.TransformFinalBlock(data, 0, data.Length);
            }
            return data;
        }
        public byte[] Encrypt(byte[] data)
        {
            return _encryptor.TransformFinalBlock(data, 0, data.Length);
        }
        public byte[] CreateSign(byte[] data)
        {
            return _rsaToSign.SignData(data, md5);
        }
        public bool VerifyData(byte[] data, byte[] signature)
        {
            if (UseSignature)
            {
                return _rsaToVerify.VerifyData(data, md5, signature);
            }
            return true;
        }
        public void ExportKeys(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            //StreamWriter fWriter = new StreamWriter(fs);
            //fWriter.WriteLine(_rsaToVerify.ToXmlString(false));
            //fWriter.WriteLine(_rsaToSign.ToXmlString(true));


            //fWriter.Write(_aes.IV.Length);
            //fWriter.Write(_aes.IV);
            //fWriter.Write(_aes.Key.Length);
            //fWriter.Write(_aes.Key);
            byte[] RsaToVerify = Encoding.UTF8.GetBytes(_rsaToVerify.ToXmlString(false));
            byte[] LenRsaToVerify = new byte[4];
            LenRsaToVerify = BitConverter.GetBytes(RsaToVerify.Length);

            byte[] RsaToSign = Encoding.UTF8.GetBytes(_rsaToVerify.ToXmlString(false));
            byte[] LenRsaToSign = new byte[4];
            LenRsaToSign = BitConverter.GetBytes(RsaToSign.Length);

            fs.Write(RsaToVerify, 0, 4);
            fs.Write(LenRsaToVerify, 0, LenRsaToVerify.Length);
            fs.Write(LenRsaToSign, 0, 4);
            fs.Write(RsaToSign, 0, RsaToSign.Length);

            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];
            LenIV = BitConverter.GetBytes(_aes.IV.Length);
            LenK = BitConverter.GetBytes(_aes.Key.Length);
            fs.Write(LenIV,0,4);
            fs.Write(_aes.IV,0, _aes.IV.Length);
            fs.Write(LenK,0,4);
            fs.Write(_aes.Key,0,_aes.Key.Length);
            //fWriter.Close();
            fs.Close();

        }
        public void TransformMessage(TextMessage message)
        {
            if (UseEncryption)
            {
                message.byteText = Encrypt(message.byteText);
            }
            if(UseSignature)
            {
                message.AddSignature(CreateSign(message.byteText));
            }
        }
        public byte[] IV
        {
            get { return _aes.IV; }            
        }
        public byte[] EncryptionKey
        {
            get { return _aes.Key; }
        }

        public RSACryptoServiceProvider RsaToSign
        {
            get
            {
                return _rsaToSign;
            }
            
        }

        public RSACryptoServiceProvider RsaToVerify
        {
            get
            {
                return _rsaToVerify;
            }
        }
    }
}
