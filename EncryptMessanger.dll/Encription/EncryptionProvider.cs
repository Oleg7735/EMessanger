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
using System.Net;
using System.Net.Sockets;

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
        public async Task<ClientClientEncryptedSession> ClientClientSenderEncryptAsync(IPEndPoint ipPoint, long dialogId, long senderId)
        {
           
                TcpListener listener = new TcpListener(ipPoint);
                listener.Start();
                TcpClient sessionUpdateConnection = listener.AcceptTcpClient(); ;
                listener.Stop();
                MessageWriter messageWriter = new MessageWriter(sessionUpdateConnection.GetStream());
                MessageReader reader = new MessageReader(sessionUpdateConnection.GetStream());
                //для шифрования симетричного ключа
                RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
                RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
                //для ЭЦП
                RSACryptoServiceProvider encryptRsaForSign = new RSACryptoServiceProvider();//предоставляет закрытый ключ для зашивровки хеша сообщений

                RSACryptoServiceProvider decryptRsaForSign = new RSACryptoServiceProvider();//предоставляет открытый ключ получателя для расшифровки хеша ЭЦП
                                                                                            //отправка публичного RSA ключа
                messageWriter.WriteMessage(new ClientAKeyMessage(decryptRsa.ToXmlString(false), dialogId, senderId));

                Message message = reader.ReadNext();
                //получение публичного RSA ключа
                if (!(message.Type == MessageType.ClientPublicKeyMessage))
                {
                    throw new Exception("Protocol error. Public key do not resived");
                }
                encryptRsa.FromXmlString(((ClientAKeyMessage)message).RsaKey);

                AesManaged aes = new AesManaged();

                aes.Mode = CipherMode.CBC;
                //шифрование симметричного ключа
                byte[] iv = encryptRsa.Encrypt(aes.IV, true);
                byte[] key = encryptRsa.Encrypt(aes.Key, true);
                //отправка симметричного ключа
                messageWriter.WriteMessage(new ClientSKeyMessage(iv, key, dialogId, senderId));
                //отправка ключа для ЭЦП
                messageWriter.WriteMessage(new ClientClientSignKeyMessage(encryptRsaForSign.ToXmlString(false), dialogId, senderId));
                //прием ключа для ЭЦП
                message = reader.ReadNext();
                if (!(message.Type == MessageType.ClientClientSignKeyMessage))
                {
                    throw new Exception("Protocol error. Sign key do not resived");
                }
                ClientClientSignKeyMessage signMessage = (ClientClientSignKeyMessage)message;
                decryptRsaForSign.FromXmlString(signMessage.RsaKey);

                List<UserVerificationData> verificationData = new List<UserVerificationData>();
                verificationData.Add(new UserVerificationData(signMessage.From, decryptRsaForSign));
                RSACryptoServiceProvider currentUserVerificationRsa = new RSACryptoServiceProvider();
                currentUserVerificationRsa.PersistKeyInCsp = false;
                currentUserVerificationRsa.ImportParameters(encryptRsaForSign.ExportParameters(false));
                verificationData.Add(new UserVerificationData(senderId, currentUserVerificationRsa));
                //ICryptoTransform encrptTransform = aes.CreateEncryptor();
                //ICryptoTransform decryptTransform = aes.CreateDecryptor();
                return new ClientClientEncryptedSession(aes, dialogId, encryptRsaForSign, verificationData);
            
        }
        public ClientClientEncryptedSession ClientClientSenderEncrypt(IPEndPoint ipPoint, long dialogId, long senderId)
        {
            TcpListener listener = new TcpListener(ipPoint);
            listener.Start();
            TcpClient sessionUpdateConnection = listener.AcceptTcpClient(); ;
            listener.Stop();
            MessageWriter messageWriter = new MessageWriter(sessionUpdateConnection.GetStream());
            MessageReader reader = new MessageReader(sessionUpdateConnection.GetStream());
            //для шифрования симетричного ключа
            RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
            //для ЭЦП
            RSACryptoServiceProvider encryptRsaForSign = new RSACryptoServiceProvider();//предоставляет закрытый ключ для зашивровки хеша сообщений
            
            RSACryptoServiceProvider decryptRsaForSign = new RSACryptoServiceProvider();//предоставляет открытый ключ получателя для расшифровки хеша ЭЦП
            //отправка публичного RSA ключа
            messageWriter.WriteMessage(new ClientAKeyMessage(decryptRsa.ToXmlString(false),dialogId,senderId));
            
            Message message = reader.ReadNext();
            //получение публичного RSA ключа
            if (!(message.Type == MessageType.ClientPublicKeyMessage))
            {
                throw new Exception("Protocol error. Public key do not resived");
            }
            encryptRsa.FromXmlString(((ClientAKeyMessage)message).RsaKey);

            AesManaged aes = new AesManaged();

            aes.Mode = CipherMode.CBC;
            //шифрование симметричного ключа
            byte[] iv = encryptRsa.Encrypt(aes.IV, true);
            byte[] key = encryptRsa.Encrypt(aes.Key, true);
            //отправка симметричного ключа
            messageWriter.WriteMessage(new ClientSKeyMessage(iv, key, dialogId, senderId));
            //отправка ключа для ЭЦП
            messageWriter.WriteMessage(new ClientClientSignKeyMessage(encryptRsaForSign.ToXmlString(false), dialogId, senderId));
            //прием ключа для ЭЦП
            message = reader.ReadNext();
            if (!(message.Type == MessageType.ClientClientSignKeyMessage))
            {
                throw new Exception("Protocol error. Sign key do not resived");
            }
            ClientClientSignKeyMessage signMessage = (ClientClientSignKeyMessage)message;
            decryptRsaForSign.FromXmlString(signMessage.RsaKey);

            List<UserVerificationData> verificationData = new List<UserVerificationData>();
            verificationData.Add(new UserVerificationData(signMessage.From, decryptRsaForSign));
            RSACryptoServiceProvider currentUserVerificationRsa = new RSACryptoServiceProvider();
            currentUserVerificationRsa.PersistKeyInCsp = false;
            currentUserVerificationRsa.ImportParameters(encryptRsaForSign.ExportParameters(false));
            verificationData.Add(new UserVerificationData(senderId, currentUserVerificationRsa));
            //ICryptoTransform encrptTransform = aes.CreateEncryptor();
            //ICryptoTransform decryptTransform = aes.CreateDecryptor();
            return new ClientClientEncryptedSession(aes, dialogId, encryptRsaForSign, verificationData);
        }


        public async Task<ClientClientEncryptedSession> ClientClientResiverEncryptAsync(IPEndPoint ipPoint, long senderId)
        {
            
                long currentUserId = senderId;
                TcpClient sessionUpdateConnection = new TcpClient();
                sessionUpdateConnection.Connect(ipPoint);
                MessageWriter messageWriter = new MessageWriter(sessionUpdateConnection.GetStream());
                MessageReader reader = new MessageReader(sessionUpdateConnection.GetStream());


                //для шифрования симетричного ключа
                RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
                RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
                //для ЭЦП
                RSACryptoServiceProvider encryptRsaForSign = new RSACryptoServiceProvider();//предоставляет закрытый ключ для зашивровки хеша сообщений
                RSACryptoServiceProvider decryptRsaForSign = new RSACryptoServiceProvider();//предоставляет открытый ключ получателя для расшифровки хеша ЭЦП

                Message message = reader.ReadNext();
                if (!(message.Type == MessageType.ClientPublicKeyMessage))
                {
                    throw new Exception("Protocol error. Public key do not resived");
                }
                ClientAKeyMessage aKeyMessage = message as ClientAKeyMessage;
                long dialogId = aKeyMessage.Dialog;
                encryptRsa.FromXmlString(((ClientAKeyMessage)message).RsaKey);
                messageWriter.WriteMessage(new ClientAKeyMessage(decryptRsa.ToXmlString(false), dialogId, currentUserId));

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
                ClientClientSignKeyMessage signMessage = (ClientClientSignKeyMessage)message;
                decryptRsaForSign.FromXmlString((signMessage).RsaKey);

                messageWriter.WriteMessage(new ClientClientSignKeyMessage(encryptRsaForSign.ToXmlString(false), dialogId, currentUserId));

                List<UserVerificationData> verificationData = new List<UserVerificationData>();
                verificationData.Add(new UserVerificationData(signMessage.From, decryptRsaForSign));

                RSACryptoServiceProvider currentUserVerificationRsa = new RSACryptoServiceProvider();
                currentUserVerificationRsa.PersistKeyInCsp = false;
                currentUserVerificationRsa.ImportParameters(encryptRsaForSign.ExportParameters(false));
                verificationData.Add(new UserVerificationData(currentUserId, currentUserVerificationRsa));
                //ICryptoTransform encrptTransform = aes.CreateEncryptor();
                //ICryptoTransform decryptTransform = aes.CreateDecryptor();
                return new ClientClientEncryptedSession(aes, dialogId, encryptRsaForSign, verificationData);
            
        }
        public ClientClientEncryptedSession ClientClientResiverEncrypt(IPEndPoint ipPoint, long currentUserId )
        {

            TcpClient sessionUpdateConnection = new TcpClient();
            sessionUpdateConnection.Connect(ipPoint);
            MessageWriter messageWriter = new MessageWriter(sessionUpdateConnection.GetStream());
            MessageReader reader = new MessageReader(sessionUpdateConnection.GetStream());
            
            
            //для шифрования симетричного ключа
            RSACryptoServiceProvider encryptRsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider decryptRsa = new RSACryptoServiceProvider();
            //для ЭЦП
            RSACryptoServiceProvider encryptRsaForSign = new RSACryptoServiceProvider();//предоставляет закрытый ключ для зашивровки хеша сообщений
            RSACryptoServiceProvider decryptRsaForSign = new RSACryptoServiceProvider();//предоставляет открытый ключ получателя для расшифровки хеша ЭЦП

            Message message =  reader.ReadNext();
            if (!(message.Type == MessageType.ClientPublicKeyMessage))
            {
                throw new Exception("Protocol error. Public key do not resived");
            }
            ClientAKeyMessage aKeyMessage = message as ClientAKeyMessage;
            long dialogId = aKeyMessage.Dialog;
            encryptRsa.FromXmlString(((ClientAKeyMessage)message).RsaKey);
            messageWriter.WriteMessage(new ClientAKeyMessage(decryptRsa.ToXmlString(false), dialogId, currentUserId));

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
            ClientClientSignKeyMessage signMessage = (ClientClientSignKeyMessage)message;
            decryptRsaForSign.FromXmlString((signMessage).RsaKey);

            messageWriter.WriteMessage(new ClientClientSignKeyMessage(encryptRsaForSign.ToXmlString(false), dialogId, currentUserId));

            List<UserVerificationData> verificationData = new List<UserVerificationData>();
            verificationData.Add(new UserVerificationData(signMessage.From, decryptRsaForSign));

            RSACryptoServiceProvider currentUserVerificationRsa = new RSACryptoServiceProvider();
            currentUserVerificationRsa.PersistKeyInCsp = false;
            currentUserVerificationRsa.ImportParameters(encryptRsaForSign.ExportParameters(false));
            verificationData.Add(new UserVerificationData(currentUserId, currentUserVerificationRsa));
            //ICryptoTransform encrptTransform = aes.CreateEncryptor();
            //ICryptoTransform decryptTransform = aes.CreateDecryptor();
            return new ClientClientEncryptedSession(aes, dialogId, encryptRsaForSign, verificationData);
        }
    }
}
