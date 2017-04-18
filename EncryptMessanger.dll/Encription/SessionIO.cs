using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Encription
{
    class SessionIO
    {       

        public ClientClientEncryptedSession LoadSession(long dialogID, string fileName)
        {
            RSACryptoServiceProvider rsaToDecryptKey;
            RSACryptoServiceProvider rsaToSign;
            RSACryptoServiceProvider rsaToVerify;
            try
            {
                CspParameters cspEncrypt = new CspParameters();
                cspEncrypt.KeyContainerName = dialogID.ToString() + EncriptionParams.RsaEncryptionKeyMark;
                rsaToDecryptKey = new RSACryptoServiceProvider(cspEncrypt);

                CspParameters cspSign = new CspParameters();
                cspSign.KeyContainerName = dialogID.ToString() + EncriptionParams.RsaSignKeyMark;
                rsaToSign = new RSACryptoServiceProvider(cspSign);

                CspParameters cspVerify = new CspParameters();
                cspVerify.KeyContainerName = dialogID.ToString() + EncriptionParams.RsaVerifyKeyMark;
                rsaToVerify = new RSACryptoServiceProvider(cspVerify);
            }
            catch(CryptographicException)
            {                
                throw new Exception("Не удалось получить rsa ключи");
            }

            BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open));
            long dialogIdFromFile;
            dialogIdFromFile = reader.ReadInt64();
            try
            {
                while (dialogIdFromFile != dialogID)
                {
                    reader.BaseStream.Seek(EncriptionParams.AesEncrIVSize + EncriptionParams.AesEncrKeySize, SeekOrigin.Current);
                    dialogIdFromFile = reader.ReadInt64();
                }
            }
            catch(EndOfStreamException)
            {
                reader.Close();
                throw new Exception("Не удалось считать ключь семмитричного шифрования для данного диалога");
            }
            byte[] aesEncrIV = reader.ReadBytes(EncriptionParams.AesEncrIVSize);
            byte[] aesEncrKey = reader.ReadBytes(EncriptionParams.AesEncrKeySize);

            reader.Close();
            byte[] IV = rsaToDecryptKey.Decrypt(aesEncrIV, true);
            byte[] key = rsaToDecryptKey.Decrypt(aesEncrKey, true);

            AesManaged aes = new AesManaged();
            aes.IV = IV;
            aes.Key = key;

            return new ClientClientEncryptedSession(aes, dialogID, rsaToSign, rsaToVerify);
        }
        //public void rewriteSewssion(long dialogId, string fileName)
        //{

        //}
        public void Save(string fileName, ClientClientEncryptedSession session)
        {
            //сохраняем Rsa приватный ключ жля подписи
            CspParameters cspSign = new CspParameters();
            cspSign.KeyContainerName = session.Dialog.ToString() + EncriptionParams.RsaSignKeyMark;
            RSACryptoServiceProvider signRsa = new RSACryptoServiceProvider(cspSign);
            signRsa.ImportParameters(session.RsaToSign.ExportParameters(true));
            signRsa.PersistKeyInCsp = true;

            //сохраняем Rsa публичный ключ для поверки ЭЦП
            CspParameters cspVerify = new CspParameters();
            cspVerify.KeyContainerName = session.Dialog.ToString() + EncriptionParams.RsaVerifyKeyMark;
            RSACryptoServiceProvider verifyRsa = new RSACryptoServiceProvider(cspVerify);
            verifyRsa.ImportParameters(session.RsaToVerify.ExportParameters(true));
            verifyRsa.PersistKeyInCsp = true;

            //CspParameters для шифрования симметричного Aes ключа
            CspParameters cspEncrypt = new CspParameters();
            cspEncrypt.KeyContainerName = session.Dialog.ToString() + EncriptionParams.RsaEncryptionKeyMark;
            RSACryptoServiceProvider rsaToEncrypt = new RSACryptoServiceProvider(cspEncrypt);
            rsaToEncrypt.PersistKeyInCsp = true;
            byte[] aesKey = session.EncryptionKey;
            byte[] IV = session.IV;
            //шифруем ключ и вектор для записи в файл
            byte[] encryptedAesKey = rsaToEncrypt.Encrypt(aesKey, true);
            byte[] encryptedIV = rsaToEncrypt.Encrypt(IV, true);
            //объединяем вектор и ключ в один массив
            byte[] encryptedAesParams = encryptedIV.Concat(encryptedAesKey).ToArray();

            long dialogIdFromFile;
            byte[] dialogIdBytes = new byte[8];

            FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);

            //stream.Read(dialogIdBytes, 0, dialogIdBytes.Length);
            
            //пока в файле есть что читать
            while (stream.Read(dialogIdBytes, 0, 8) != 0) 
            {
                dialogIdFromFile = BitConverter.ToInt64(dialogIdBytes, 0);
                //если наткнулись на запись с нужным dialogId перезаписываем aes параметры
                if(dialogIdFromFile == session.Dialog)
                {                    
                    stream.Write(encryptedAesParams, 0, encryptedAesParams.Length);
                    stream.Close();
                    return;
                }
            }
            //если записи с таким dialogId не найдены добавляем новую запись
            byte[] bytesToWrite = BitConverter.GetBytes(session.Dialog).Concat(encryptedAesParams).ToArray();
            stream.Write(bytesToWrite, 0, bytesToWrite.Length);
            stream.Close();

        }
    }
}
