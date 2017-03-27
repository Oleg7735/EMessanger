using EncryptMessanger.dll.Messages;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Authentification
{
    /// <summary>
    /// Делегат функции, вызываемой после получения регистрационных данных, для проверки
    ///возможности регистрации данного пользователя в базе данных сервера и регистрации ,если она возможна.
    /// </summary>
    /// <param name="login">Логин регистрируемого пользователя</param>
    /// <param name="password">Пароль регистрируемого пользователя</param>
    /// /// <param name="error">Текст ошибки при неудачной регистрации</param>
    /// <returns>Возвращает true если регестрация прошла успешно, иначе false</returns>

    public delegate bool RegistrationDelegate(string login, byte[] password, out string error);
    public class ServerRegistrationHandler
    {
        public bool HandleRegistration(MessageWriter writer, MessageReader reader, RegistrationMessage registrMessage, RegistrationDelegate RegistrationHandler)
        {
            if(String.IsNullOrEmpty(registrMessage.Login))
            {
                writer.WriteMessage(new RegistrationErrorMessage("Некорректный логин"));
            }
            if (registrMessage.BytePassword.Length==0)
            {
                writer.WriteMessage(new RegistrationErrorMessage("Некорректный пароль"));
            }
            string error;
            if(RegistrationHandler(registrMessage.Login, registrMessage.BytePassword, out error))
            {
                writer.WriteMessage(new RegistrationSuccessMessage());
                return true;
            }
            else
            {
                writer.WriteMessage(new RegistrationErrorMessage(error));
            }
            return false;
        }
    }
}
