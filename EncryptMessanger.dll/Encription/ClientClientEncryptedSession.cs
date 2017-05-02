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
                ICryptoTransform decryptor = _aes.CreateDecryptor();
                byte[] decryptedData = decryptor.TransformFinalBlock(data, 0, data.Length);
                
                return decryptedData;
            }
            return data;
        }
        /// <summary>
        /// Метод, подготавливающий сессию к новому сеансу потокового шифроваия с помощью метода EncryptAsStream
        /// Следует вызывать перед выше указанным методами.
        /// </summary>
        public void InitStreamEncryptor()
        {            
            _encryptor = _aes.CreateEncryptor();
        }

        /// <summary>
        /// Метод, подготавливающий сессию к новому сеансу потокового дешифроваия с помощью метода DecryptAsStream
        /// Следует вызывать перед выше указанным методами.
        /// </summary>
        public void InitStreamDecryptor()
        {
            _decryptor = _aes.CreateDecryptor();
        }

        public byte[] Encrypt(byte[] data)
        {
            //byte[] startIV = _aes.IV;
            ICryptoTransform encryptor = _aes.CreateEncryptor();
            byte[] encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length);
            
            //_aes.IV = startIV;
            return encryptedData;
        }
        public ICryptoTransform GetEncryptor()
        {
            return _aes.CreateEncryptor();
        }
        public ICryptoTransform GetDecryptor()
        {
            return _aes.CreateDecryptor();
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
        public byte[] SignFile(Stream file)
        {
            return _rsaToSign.SignData(file, md5);
        }
        public bool VerifyFile(Stream file, long userId, byte[] signature)
        {
            RSACryptoServiceProvider rsa = GetVerificationRsa(userId);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(file);
            return rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("MD5") , signature);
        }
        public byte[] EncryptBytesAsStream(byte[] bytes)
        {
            byte[] res = new byte[bytes.Length];
            //int transed = 0;
            //for (int i = 0; i < 256; i += transed)
            //{
            //    transed = _encryptor.TransformBlock(bytes, i, 16, res, i);
            //}            
            _encryptor.TransformBlock(bytes, 0, bytes.Length, res, 0);
            return res;
            //return _encryptor.TransformFinalBlock(bytes,0,bytes.Length);
            
        }
        public byte[] DecryptBytesAsStream(byte[] bytes)
        {
            byte[] res = new byte[bytes.Length];
            //int transed = 0;

            //int s = 0;
            //for (int i = 0; i < 256; i += transed)
            //{
            //    transed = _decryptor.TransformBlock(bytes, i, 16, res, i);
            //    s += 16;
            //}
            _decryptor.TransformBlock(bytes, 0, bytes.Length, res, 0);
            //if (p < bytes.Length)
            //{
            //    _decryptor.TransformBlock(bytes, bytes.Length, bytes.Length - p, res, p);
            //}
            return res;
            //return _decryptor.TransformFinalBlock(bytes, 0, bytes.Length); 
        }

        public ClientClientEncryptedSession GetCopy()
        {
            return new ClientClientEncryptedSession(_aes, Dialog, _rsaToSign, new List<UserVerificationData> (VerificationData));
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
