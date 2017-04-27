using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Encription
{
    public class SessionIO
    {
        //private int _recordSize = EncriptionParams.AesEncrIVSize + EncriptionParams.AesEncrKeySize + EncriptionParams.RsaVerifyKeyXmlSize;
        public ClientClientEncryptedSession LoadSession(long dialogID, long ownerId, string fileName)
        {
            RSACryptoServiceProvider rsaToDecryptKey;
            RSACryptoServiceProvider rsaToSign;
            RSACryptoServiceProvider rsaToVerify;
            
            try
            {
                CspParameters cspEncrypt = new CspParameters();
                cspEncrypt.KeyContainerName = EncriptionParams.GetRsaToEncryptAesKeyContainerName(dialogID, ownerId);
                rsaToDecryptKey = new RSACryptoServiceProvider(cspEncrypt);

                CspParameters cspSign = new CspParameters();
                cspSign.KeyContainerName = EncriptionParams.GetSignKeyContainerName(dialogID, ownerId);
                rsaToSign = new RSACryptoServiceProvider(cspSign);

                //CspParameters cspVerify = new CspParameters();
                //cspVerify.KeyContainerName = dialogID.ToString() + EncriptionParams.RsaVerifyKeyMark;
                //rsaToVerify = new RSACryptoServiceProvider(cspVerify);
            }
            catch(CryptographicException)
            {                
                throw new Exception("Не удалось получить rsa ключи");
            }

            BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open));
            long dialogIdFromFile;
            int skipRecordLength;
            dialogIdFromFile = reader.ReadInt64();
            try
            {
                while (dialogIdFromFile != dialogID)
                {
                    skipRecordLength = reader.ReadInt32();
                    reader.BaseStream.Seek(skipRecordLength, SeekOrigin.Current);
                    dialogIdFromFile = reader.ReadInt64();
                }
            }
            catch(EndOfStreamException)
            {
                reader.Close();
                throw new Exception("Не удалось считать ключи шифрования для данного диалога");
            }
            int recordLength = reader.ReadInt32();
            byte[] aesEncrIV = reader.ReadBytes(EncriptionParams.AesEncrIVSize);
            byte[] aesEncrKey = reader.ReadBytes(EncriptionParams.AesEncrKeySize);
            List<UserVerificationData> verifyData = new List<UserVerificationData>();
            int verifyDataCount = reader.ReadInt32();
            long userId;
            string rsaKeyString;
            for(int i = 0; i < verifyDataCount; i++)
            {
                userId = reader.ReadInt64();
                byte[] rsaVerifyKey = reader.ReadBytes(EncriptionParams.RsaVerifyKeyXmlSize);
                rsaToVerify = new RSACryptoServiceProvider();
                rsaKeyString = Encoding.UTF8.GetString(rsaVerifyKey);
                rsaToVerify.FromXmlString(rsaKeyString);
                rsaToVerify.PersistKeyInCsp = false;
                verifyData.Add(new UserVerificationData(userId, rsaToVerify));
            } 

            reader.Close();
            byte[] IV = rsaToDecryptKey.Decrypt(aesEncrIV, true);
            byte[] key = rsaToDecryptKey.Decrypt(aesEncrKey, true);

            AesManaged aes = new AesManaged();
            aes.IV = IV;
            aes.Key = key;

            return new ClientClientEncryptedSession(aes, dialogID, rsaToSign, verifyData);
        }
        //public void rewriteSewssion(long dialogId, string fileName)
        //{

        //}
        public void Save(string fileName, long ownerId, ClientClientEncryptedSession session)
        {
            //сохраняем Rsa приватный ключ жля подписи
            CspParameters cspSign = new CspParameters();
            cspSign.KeyContainerName = EncriptionParams.GetSignKeyContainerName(session.Dialog, ownerId);
            RSACryptoServiceProvider signRsa = new RSACryptoServiceProvider(cspSign);
            signRsa.ImportParameters(session.RsaToSign.ExportParameters(true));
            signRsa.PersistKeyInCsp = true;

            ////сохраняем Rsa публичный ключ для поверки ЭЦП
            //CspParameters cspVerify = new CspParameters();
            //cspVerify.KeyContainerName = session.Dialog.ToString() + EncriptionParams.RsaVerifyKeyMark;
            //RSACryptoServiceProvider verifyRsa = new RSACryptoServiceProvider(cspVerify);
            //verifyRsa.ImportParameters(session.RsaToVerify.ExportParameters(false));
            MemoryStream record = new MemoryStream();
            
            //CspParameters для шифрования симметричного Aes ключа
            CspParameters cspEncrypt = new CspParameters();
            cspEncrypt.KeyContainerName = EncriptionParams.GetRsaToEncryptAesKeyContainerName(session.Dialog, ownerId);
            RSACryptoServiceProvider rsaToEncrypt = new RSACryptoServiceProvider(cspEncrypt);
            rsaToEncrypt.PersistKeyInCsp = true;
            byte[] aesKey = session.EncryptionKey;
            byte[] IV = session.IV;
            //шифруем ключ и вектор для записи в файл
            byte[] encryptedAesKey = rsaToEncrypt.Encrypt(aesKey, true);
            byte[] encryptedIV = rsaToEncrypt.Encrypt(IV, true);
            //объединяем вектор и ключ в один массив
            //byte[] encryptedAesParams = encryptedIV.Concat(encryptedAesKey).ToArray();
            
            byte[] rsaVerifyDataCount;
            //byte[] rsaVerifyBytes;
            byte[] currentRsaVerifyClient;
            byte[] currentRsaVerifyKey;
            rsaVerifyDataCount = BitConverter.GetBytes(session.VerificationData.Length);

            
            //record.Write(savingSessionDialogBytes, 0, savingSessionDialogBytes.Length);
            record.Write(encryptedIV, 0, encryptedIV.Length);
            record.Write(encryptedAesKey, 0, encryptedAesKey.Length);
            record.Write(rsaVerifyDataCount, 0, rsaVerifyDataCount.Length);
            foreach (UserVerificationData verificationData in session.VerificationData)
            {
                currentRsaVerifyClient = BitConverter.GetBytes(verificationData.UserID);
                currentRsaVerifyKey = Encoding.UTF8.GetBytes(verificationData.RsaToVerify.ToXmlString(false));

                record.Write(currentRsaVerifyClient, 0, currentRsaVerifyClient.Length);
                record.Write(currentRsaVerifyKey, 0, currentRsaVerifyKey.Length);
                //rsaVerifyBytes = rsaVerifyBytes.Concat(verificationData);
            }
            //rsaVerifyBytes = Encoding.UTF8.GetBytes(session.RsaToVerify.ToXmlString(false));
            

            FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);

            //stream.Read(dialogIdBytes, 0, dialogIdBytes.Length);
            byte[] savingSessionDialogBytes = BitConverter.GetBytes(session.Dialog);
            //пока в файле есть что читать
            byte[] recordBytes = record.GetBuffer();
            byte[] recordBytesLength = BitConverter.GetBytes(recordBytes.Length);

            //переменные в которые вносится dialogId просматриваемых в файле записей
            long dialogIdFromFile;
            byte[] dialogIdBytes = new byte[8];
            byte[] skipRecordLengthBytes = new byte[4];
            int skipRecordLength;

            while (stream.Read(dialogIdBytes, 0, 8) != 0) 
            {
                dialogIdFromFile = BitConverter.ToInt64(dialogIdBytes, 0);
                //если наткнулись на запись с нужным dialogId перезаписываем aes параметры
                if(dialogIdFromFile == session.Dialog)
                {
                    //stream.Write(encryptedAesParams, 0, encryptedAesParams.Length);                    
                    //stream.Write(rsaVerifyBytes, 0, rsaVerifyBytes.Length);
                    //stream.Write(savingSessionDialogBytes, 0, savingSessionDialogBytes.Length);
                    stream.Write(recordBytesLength, 0, recordBytesLength.Length);
                    stream.Write(recordBytes, 0, recordBytes.Length);
                    stream.Close();
                    return;
                }
                stream.Read(skipRecordLengthBytes, 0, 4);
                skipRecordLength = BitConverter.ToInt32(skipRecordLengthBytes, 0);
                stream.Seek(skipRecordLength, SeekOrigin.Current);
            }

            //если записи с таким dialogId не найдены добавляем новую запись
            stream.Write(savingSessionDialogBytes, 0, savingSessionDialogBytes.Length);
            stream.Write(recordBytesLength, 0, recordBytesLength.Length);
            stream.Write(recordBytes, 0, recordBytes.Length);
            stream.Close(); 

        }
    }
}
