using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptMessanger.dll.Messages;
using System.Security.Cryptography;

namespace EncryptMessanger.dll.Authentification
{/// <summary>
/// Класс, инкапсулирующий регистрацию пользователя
/// </summary>
    public class ClientRegistrator
    {
        private string _lastError;

        public string LastError
        {
            get
            {
                return _lastError;
            }

            private set
            {
                _lastError = value;
            }
        }
        /// <summary>
        /// Метод для регистрации клиента.
        /// </summary>
        /// <param name="writer">MessageWriter, связанный с сервером</param>
        /// <param name="reader">MessageReader, связанный с сервером</param>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Хешь от пароля пользователя</param>
        /// <returns>Признак удачного окончания регистрации. Если регистрация не удалась, устанавливает в поле lastError текст ошибки</returns>
        public bool Registrate(MessageWriter writer, MessageReader reader, string login, string stringPassword)
        {
            MD5 md5 = MD5.Create();
            byte[] password = md5.ComputeHash(Encoding.UTF8.GetBytes(stringPassword));
            writer.WriteMessage(new RegistrationMessage(login, password));
            Message responceMessage = reader.ReadNext();
            if(responceMessage.Type != MessageType.AuthMessage)
            {
                throw new ArgumentException("Protocol error! Auth message expected? but "+responceMessage.Type.ToString()+" received");
            }
            responceMessage = reader.ReadNext();
            switch (responceMessage.Type)
            {
                case MessageType.RegistrationSuccessMessage:
                    {
                        return true;
                    }
                case MessageType.RegistrationErrorMessage:
                    {
                        RegistrationErrorMessage errorMessage = responceMessage as RegistrationErrorMessage;
                        LastError = errorMessage.Text;
                        return false;
                    }
            }
            throw new Exception("Unexpected registration responce message type. "+MessageType.RegistrationErrorMessage.ToString()+
                " or "+MessageType.RegistrationSuccessMessage.ToString()+" expected"+" but "+responceMessage.Type.ToString()+" resived");
        }
    }
}
