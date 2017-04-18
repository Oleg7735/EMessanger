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
    public class SessionIOTest
    {
        [Test]
        public void SaveLoadTest()
        {
            SessionIO io = new SessionIO();
            AesManaged aes = new AesManaged();
            RSACryptoServiceProvider rsaSign = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsaVerify = new RSACryptoServiceProvider();
            ClientClientEncryptedSession session= new ClientClientEncryptedSession(aes, 4245, rsaSign, rsaVerify);

            io.Save(@"C:\Users\home\Documents\Visual Studio 2015\Projects\EncryptMessangerClient\EncryptMessangerClient\bin\Debug\keysTest.test", session);
            ClientClientEncryptedSession sessionTest = io.LoadSession(4245, @"C:\Users\home\Documents\Visual Studio 2015\Projects\EncryptMessangerClient\EncryptMessangerClient\bin\Debug\keysTest.test");
            bool dialogIdEquals = sessionTest.Dialog == session.Dialog;
            bool IVEquals = sessionTest.IV.SequenceEqual(session.IV);
            bool KeyEquals = sessionTest.EncryptionKey.SequenceEqual(session.EncryptionKey);
            bool rsaSignEquals = sessionTest.RsaToSign.ToXmlString(true).Equals(session.RsaToSign.ToXmlString(true));
            string rsaVerTest = sessionTest.RsaToVerify.ToXmlString(false);
            string rsaVer = session.RsaToVerify.ToXmlString(false);
            bool rsaToVerifyEquals = rsaVerTest.Equals(rsaVer);
            Assert.That(dialogIdEquals && IVEquals && KeyEquals && rsaSignEquals && rsaToVerifyEquals);

        }
        [Test]
        public void ResaveLoadTest()
        {
            SessionIO io = new SessionIO();
            AesManaged aes = new AesManaged();
            RSACryptoServiceProvider rsaSign = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsaVerify = new RSACryptoServiceProvider();
            ClientClientEncryptedSession session = new ClientClientEncryptedSession(aes, 4218, rsaSign, rsaVerify);
            io.Save(@"C:\Users\home\Documents\Visual Studio 2015\Projects\EncryptMessangerClient\EncryptMessangerClient\bin\Debug\keysTest.test", session);

            
            AesManaged changedAes = new AesManaged();
            RSACryptoServiceProvider changedSign = new RSACryptoServiceProvider();
            RSACryptoServiceProvider changedVerify = new RSACryptoServiceProvider();
            ClientClientEncryptedSession changedSession = new ClientClientEncryptedSession(changedAes, 4218, changedSign, changedVerify);
            io.Save(@"C:\Users\home\Documents\Visual Studio 2015\Projects\EncryptMessangerClient\EncryptMessangerClient\bin\Debug\keysTest.test", changedSession);

            ClientClientEncryptedSession sessionTest = io.LoadSession(4218, @"C:\Users\home\Documents\Visual Studio 2015\Projects\EncryptMessangerClient\EncryptMessangerClient\bin\Debug\keysTest.test");
            bool dialogIdEquals = sessionTest.Dialog == session.Dialog;
            bool IVEquals = sessionTest.IV.SequenceEqual(changedSession.IV);
            bool KeyEquals = sessionTest.EncryptionKey.SequenceEqual(changedSession.EncryptionKey);
            bool rsaSignEquals = sessionTest.RsaToSign.ToXmlString(true).Equals(changedSession.RsaToSign.ToXmlString(true));
            string rsaVerTest = sessionTest.RsaToVerify.ToXmlString(false);
            string rsaVer = changedSession.RsaToVerify.ToXmlString(false);
            bool rsaToVerifyEquals = rsaVerTest.Equals(rsaVer);
            Assert.That(dialogIdEquals && IVEquals && KeyEquals && rsaSignEquals && rsaToVerifyEquals);
        }
    }
}
