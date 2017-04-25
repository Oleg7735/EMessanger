using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using EncryptMessanger.dll.Messages;
using System.Diagnostics;

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
        //RSACryptoServiceProvider _rsaToVerify;
        List<UserVerificationData> _verificationData;

        long _dialogId;
        public long Dialog
        {
            get { return _dialogId; }
        }

        private RSACryptoServiceProvider GetVerificationRsa(long authorId)
        {
            return _verificationData.Find(x => x.UserID == authorId).RsaToVerify;
        }

        public ClientClientEncryptedSession(AesManaged aes, long dialogId, RSACryptoServiceProvider rsaToSign, List<UserVerificationData> verificationData)
        {
            _dialogId = dialogId;
            //_encryptor = encryptor;
            //_decryptor = decryptor;
            _encryptor = aes.CreateEncryptor();
            _decryptor = aes.CreateDecryptor();
            _aes = aes;

            _rsaToSign = rsaToSign;
            _verificationData = verificationData;
            UseEncryption = true;
            UseSignature = true;
        }
        public byte[] Dectypt(byte[] data)
        {
            if (UseEncryption)
            {
                byte[] decryptedData = _decryptor.TransformFinalBlock(data, 0, data.Length);
                _decryptor = _aes.CreateDecryptor();
                return decryptedData;
            }
            return data;
        }
        public byte[] Encrypt(byte[] data)
        {
            //byte[] startIV = _aes.IV;
            byte[] encryptedData = _encryptor.TransformFinalBlock(data, 0, data.Length);
            _encryptor = _aes.CreateEncryptor();
            //_aes.IV = startIV;
            return encryptedData;
        }
        public byte[] CreateSign(byte[] data)
        {
            
            return _rsaToSign.SignData(data, md5);
        }
        /// <summary>
        /// Проверяет ЭЦП для данного сообщения
        /// </summary>
        /// <param name="data">Байты текта сообщения</param>
        /// <param name="signature">ЭЦП, прикрепленная к сообщению</param>
        /// <param name="authorInfo">Идентификатор автора сообщения(чтобы знать чей ключ использовать для расшифровки подписи)</param>
        /// <returns></returns>
        public bool VerifyData(byte[] data, byte[] signature, long authorId)
        {
            if (UseSignature)
            {                
                return GetVerificationRsa(authorId).VerifyData(data, md5, signature.Take(128).ToArray());
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
            byte[] verificationDataCount = BitConverter.GetBytes(_verificationData.Count);
            fs.Write(verificationDataCount, 0, verificationDataCount.Length);
            foreach (UserVerificationData verificationDada in _verificationData)
            {
                byte[] RsaToVerify = Encoding.UTF8.GetBytes(verificationDada.RsaToVerify.ToXmlString(false));
                byte[] LenRsaToVerify = new byte[4];
                LenRsaToVerify = BitConverter.GetBytes(RsaToVerify.Length);

                fs.Write(LenRsaToVerify, 0, LenRsaToVerify.Length);
                fs.Write(RsaToVerify, 0, RsaToVerify.Length);
                              
            }

            byte[] RsaToSign = Encoding.UTF8.GetBytes(_rsaToSign.ToXmlString(true));
            byte[] LenRsaToSign = new byte[4];
            LenRsaToSign = BitConverter.GetBytes(RsaToSign.Length);


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

        public UserVerificationData[] VerificationData
        {
            get
            {
                return _verificationData.ToArray();
            }
        }
    }
}
