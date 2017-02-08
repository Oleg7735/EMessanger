using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EncryptMessanger.dll.Messages;
using System.Security.Cryptography;
using System.Globalization;
using System.Diagnostics;

namespace EncryptMessanger.dll.Encription
{
    public class EncryptionProvider
    {
        public void ClientServerEncrypt(MessageWriter messageWriter, MessageReader reader)
        {
            RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
           
            Message message = reader.ReadNext();
            if (!(message.Type == MessageType.PublicKeyMessage))
            {
                throw new Exception("Protocol error. Public key do not resived");
            }            
            encryptRsa.FromXmlString(((AKeyMessage)message).RsaKey);
            messageWriter.WriteMessage(new AKeyMessage(decryptRsa.ToXmlString(false)));

            message = reader.ReadNext();
            //reader.Read();
            if (!(message.Type == MessageType.SymKeyMessage))
            {
                throw new Exception("Protocol error. Symetric key do not resived");
            }
            AesManaged aes = new AesManaged();
            //aes.BlockSize = 256;
            //aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;

            SKeyMessage sMessage = message as SKeyMessage;
            aes.IV = decryptRsa.Decrypt(sMessage.IV, true);
            //Debug.WriteLine(Encoding.UTF8.GetString(aes.IV));
            
            //byte[] key= s.Split('-').Select(_ => byte.Parse(_, NumberStyles.HexNumber)).ToArray<byte>();
            aes.Key = decryptRsa.Decrypt(sMessage.Key,true);
            //Debug.WriteLine(Encoding.UTF8.GetString(aes.Key));
            ICryptoTransform encrptTransform = aes.CreateEncryptor();
            ICryptoTransform decryptTransform = aes.CreateDecryptor();
            messageWriter.ProtectStream(encrptTransform);
            reader.ProtectStream(decryptTransform);
            //messageWriter.WriteMessage(new AKeyMessage());
        }
        public void ServerClientEncrypt(MessageWriter messageWriter, MessageReader reader)
        {
            RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
            messageWriter.WriteMessage(new AKeyMessage(decryptRsa.ToXmlString(false)));

            Message message = reader.ReadNext();
            if (!(message.Type == MessageType.PublicKeyMessage))
            {
                throw new Exception("Protocol error. Public key do not resived");
            }
            encryptRsa.FromXmlString(((AKeyMessage)message).RsaKey);

            AesManaged aes = new AesManaged();
            //aes.BlockSize = 256;
            //aes.KeySize = 256;

            aes.Mode = CipherMode.CBC;

            //Debug.WriteLine(Encoding.UTF8.GetString(aes.IV));
            //Debug.WriteLine(Encoding.UTF8.GetString(aes.Key));
            byte[] iv = encryptRsa.Encrypt(aes.IV, true);
            byte[] key = encryptRsa.Encrypt(aes.Key, true);

            messageWriter.WriteMessage(new SKeyMessage(iv, key));

            ICryptoTransform encrptTransform = aes.CreateEncryptor();
            ICryptoTransform decryptTransform = aes.CreateDecryptor();
            messageWriter.ProtectStream(encrptTransform);
            reader.ProtectStream(decryptTransform);
        }
        public ClientClientEncryptedSession ClientClientSenderEncrypt(MessageWriter messageWriter, MessageReader reader, string senderLogin, string reciverLogin)
        {
            //для шифрования симетричного ключа
            RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
            //для ЭЦП
            RSACryptoServiceProvider encryptRsaForSign = new RSACryptoServiceProvider();//предоставляет закрытый ключ для зашивровки хеша сообщений
            RSACryptoServiceProvider decryptRsaForSign = new RSACryptoServiceProvider();//предоставляет открытый ключ получателя для расшифровки хеша ЭЦП
            //отправка публичного RSA ключа
            messageWriter.WriteMessage(new ClientAKeyMessage(decryptRsa.ToXmlString(false),reciverLogin,senderLogin));
            
            Message message = reader.ReadNext();
            //получение публичного RSA ключа
            if (!(message.Type == MessageType.ClientPublicKeyMessage))
            {
                throw new Exception("Protocol error. Public key do not resived");
            }
            encryptRsa.FromXmlString(((ClientAKeyMessage)message).RsaKey);

            AesManaged aes = new AesManaged();

            aes.Mode = CipherMode.CBC;

            byte[] iv = encryptRsa.Encrypt(aes.IV, true);
            byte[] key = encryptRsa.Encrypt(aes.Key, true);
            //отправка симметричного ключа
            messageWriter.WriteMessage(new ClientSKeyMessage(iv, key, reciverLogin, senderLogin));
            //отправка ключа для ЭЦП
            messageWriter.WriteMessage(new ClientClientSignKeyMessage(encryptRsaForSign.ToXmlString(false), reciverLogin, senderLogin));
            //прием ключа для ЭЦП
            message = reader.ReadNext();
            if (!(message.Type == MessageType.ClientClientSignKeyMessage))
            {
                throw new Exception("Protocol error. Sign key do not resived");
            }
            decryptRsaForSign.FromXmlString(((ClientClientSignKeyMessage)message).RsaKey);

            //ICryptoTransform encrptTransform = aes.CreateEncryptor();
            //ICryptoTransform decryptTransform = aes.CreateDecryptor();
            return new ClientClientEncryptedSession(aes, reciverLogin, encryptRsaForSign, decryptRsaForSign);
        }
        public ClientClientEncryptedSession ClientClientResiverEncrypt(MessageWriter messageWriter, MessageReader reader, string currentUserLogin, ClientAKeyMessage aKeyMessage)
        {
            //для шифрования симетричного ключа
            string anotherUserLogin = aKeyMessage.From;
            RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
            //для ЭЦП
            RSACryptoServiceProvider encryptRsaForSign = new RSACryptoServiceProvider();//предоставляет закрытый ключ для зашивровки хеша сообщений
            RSACryptoServiceProvider decryptRsaForSign = new RSACryptoServiceProvider();//предоставляет открытый ключ получателя для расшифровки хеша ЭЦП

            Message message = aKeyMessage; //= reader.ReadNext();
            //if (!(message.Type == MessageType.ClientPublicKeyMessage))
            //{
            //    throw new Exception("Protocol error. Public key do not resived");
            //}
            encryptRsa.FromXmlString(((ClientAKeyMessage)message).RsaKey);
            messageWriter.WriteMessage(new ClientAKeyMessage(decryptRsa.ToXmlString(false),anotherUserLogin, currentUserLogin));

            message = reader.ReadNext();
            if (!(message.Type == MessageType.ClientSymKeyMessage))
            {
                throw new Exception("Protocol error. Symetric key do not resived");
            }
            AesManaged aes = new AesManaged();
            aes.Mode = CipherMode.CBC;

            ClientSKeyMessage sMessage = message as ClientSKeyMessage;
            aes.IV = decryptRsa.Decrypt(sMessage.IV, true);

            aes.Key = decryptRsa.Decrypt(sMessage.Key, true);

            message = reader.ReadNext();
            if (!(message.Type == MessageType.ClientClientSignKeyMessage))
            {
                throw new Exception("Protocol error. Sign key do not resived");
            }
            decryptRsaForSign.FromXmlString(((ClientClientSignKeyMessage)message).RsaKey);
            messageWriter.WriteMessage(new ClientClientSignKeyMessage(encryptRsaForSign.ToXmlString(false), anotherUserLogin, currentUserLogin));
            

            //ICryptoTransform encrptTransform = aes.CreateEncryptor();
            //ICryptoTransform decryptTransform = aes.CreateDecryptor();
            return new ClientClientEncryptedSession(aes, anotherUserLogin, encryptRsaForSign, decryptRsaForSign );
        }
    }
}
