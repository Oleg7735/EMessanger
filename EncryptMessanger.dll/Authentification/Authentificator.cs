using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptMessanger.dll.Messages;
/*using EncryptMessanger.dll.DataBase.Repositories;
using EncryptMessanger.dll.DataBase;*/
using System.Security.Cryptography;

namespace EncryptMessanger.dll.Authentification
{
    public delegate bool UserPredicate(string login, byte[] password);
    public class Authentificator
    {
        
        private string _login;
        private long _clientId;


        public string CurrentError;
        public string Login
        {
            get { return _login; }
        }

        public long ClientId
        {
            get
            {
                return _clientId;
            }
            
        }

        private byte[] GetUserPassword(long userId)
        {
            return new byte[] { 1, 2, 3, 4, 5, 6, 6 };
        }
        //сравнивает заданные логин и пароль с логином и паролем, ханящимеся в базе данных
        /*private bool CompareInfo(string login, byte[] password)
        {
            UserRepository ur = new UserRepository();
            Users user = ur.GetUserByLogin(login);
            if(user == null)
            {
                return false;
            }
            //MD5 md5 = MD5.Create();
            byte[] hash = user.hash.ToArray();
            byte[] hash16 = new byte[16];
            Array.Copy(hash,hash16,16);

            if (password.SequenceEqual(hash16))
            { 
                _login = login;
                return true;
            }
            return false;
        }*/
        //метод для аутентификации со стороны клиента
        public bool ClientAuth(MessageWriter messageWriter, MessageReader reader, string login, string password)
        {
            //MD5 md51 = MD5.Create();
            //UserRepository ur = new UserRepository();
            //Users user1 = ur.GetItem(1);
            //user1.hash = md51.ComputeHash(Encoding.UTF8.GetBytes("111"));
            //byte[] hash1 = user1.hash.ToArray();
            //Users user2 = ur.GetItem(2);
            //user2.hash = md51.ComputeHash(Encoding.UTF8.GetBytes("222"));

            //Users user3 = ur.GetItem(3);
            //user3.hash = md51.ComputeHash(Encoding.UTF8.GetBytes("333"));
            //ur.Save();

            Messages.Message message;
            do
            {
                message = reader.ReadNext();
            } while (message.Type!=MessageType.AuthMessage);
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            messageWriter.WriteMessage(new AuthResponceMessage(login, hash));
            message = reader.ReadNext();
            switch (message.Type)
            {
                case MessageType.AuthSuccessMessage:
                    {
                        AuthSuccessMessage authSuccessMessage = (AuthSuccessMessage) message;
                        _clientId = authSuccessMessage.UserId;
                        _login = login;
                        return true;
                    }
                case MessageType.AuthErrorMessage:
                    {
                        AuthErrorMessage authError = message as AuthErrorMessage;
                        CurrentError = authError.Text;
                        //throw new Exception(authError.Text);
                        return false;
                    }
            }
            return false;
        }
        //метод для аутентификации со стороны сервера
        public bool ServerAuth(MessageWriter messageWriter, MessageReader reader, UserPredicate CompareInfo, AuthResponceMessage authResponse)
        {
            
            /*Messages.Message message = reader.ReadNext();
            if (message.Type != MessageType.AuthResponceMessage)
            {
                if (message.Type == MessageType.EndStreamMessage)
                {
                    throw new ObjectDisposedException("Authentification aborted.");
                }
                else
                {
                    throw new Exception("Protocol error. AuthResponceMessage expected but " + message.Type.ToString("g") + " recived");
                }
                return false;
            }
            AuthResponceMessage authResponse = message as AuthResponceMessage;*/

            if(CompareInfo(authResponse.Login, authResponse.Password))
            {
                //messageWriter.WriteMessage(new AuthSuccessMessage());
                _login = authResponse.Login;
                return true;
            }
            messageWriter.WriteMessage(new AuthErrorMessage("Неверный логин или пароль"));
            return false;
        }        
    }
}
