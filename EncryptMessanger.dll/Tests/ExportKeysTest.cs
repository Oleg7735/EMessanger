using EncryptMessanger.dll.Encription;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Tests
{
    [TestFixture]
    class ExportKeysTest
    {
        [Test]
        public void ExpotrImportTest()
        {
            string fileName = @"D:\test\keys.kf";
            AesManaged aes = new AesManaged();
            long dialogId = 5345;
            long userId = 3763;
            long anotherUserId = 42;
            RSACryptoServiceProvider signProvider = new RSACryptoServiceProvider();
            signProvider.PersistKeyInCsp = false;
            List<UserVerificationData> verificationData = new List<UserVerificationData>();
            RSACryptoServiceProvider verifyProvider = new RSACryptoServiceProvider();
            verifyProvider.PersistKeyInCsp = false;
            verificationData.Add(new UserVerificationData(anotherUserId, verifyProvider));
            ClientClientEncryptedSession session = new ClientClientEncryptedSession(aes, dialogId, signProvider, verificationData);
            session.ExportKeys(fileName, userId);
            ClientClientEncryptedSession anotherSession = new ClientClientEncryptedSession(new AesManaged(), dialogId, null, new List<UserVerificationData>());
            anotherSession.ImportKeys(fileName, userId);
            Assert.IsTrue(session.SessionEquals(anotherSession));
        }
    }
}
