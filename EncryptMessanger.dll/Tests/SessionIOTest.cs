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
        private string keyFileName = @"C:\Users\home\Documents\Visual Studio 2015\Projects\EncryptMessangerClient\EncryptMessangerClient\bin\Debug\keysTest.test";
        [Test]
        public void SaveLoadTest()
        {
            SessionIO io = new SessionIO();
            AesManaged aes = new AesManaged();
            RSACryptoServiceProvider rsaSign = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsaVerify = new RSACryptoServiceProvider();
            rsaVerify.PersistKeyInCsp = false;

            RSACryptoServiceProvider enotherRsaVerify = new RSACryptoServiceProvider();
            enotherRsaVerify.PersistKeyInCsp = false;

            List<UserVerificationData> data = new List<UserVerificationData>();
            data.Add(new UserVerificationData(34, rsaVerify));
            data.Add(new UserVerificationData(76, enotherRsaVerify));

            ClientClientEncryptedSession session= new ClientClientEncryptedSession(aes, 4245, rsaSign, data);
            io.Save(keyFileName, 76, session);
            ClientClientEncryptedSession sessionTest = io.LoadSession(4245,76, keyFileName);
            bool dialogIdEquals = sessionTest.Dialog == session.Dialog;
            bool IVEquals = sessionTest.IV.SequenceEqual(session.IV);
            bool KeyEquals = sessionTest.EncryptionKey.SequenceEqual(session.EncryptionKey);
            bool rsaSignEquals = sessionTest.RsaToSign.ToXmlString(true).Equals(session.RsaToSign.ToXmlString(true));
            string rsaVerTest1 = sessionTest.VerificationData[0].RsaToVerify.ToXmlString(false);
            string rsaVerTest2 = sessionTest.VerificationData[1].RsaToVerify.ToXmlString(false);
            string rsaVer1 = session.VerificationData[0].RsaToVerify.ToXmlString(false);
            string rsaVer2 = session.VerificationData[1].RsaToVerify.ToXmlString(false);
            bool rsaToVerifyEquals1 = rsaVerTest1.Equals(rsaVer1);
            bool rsaToVerifyEquals2 = rsaVerTest2.Equals(rsaVer2);
            Assert.That(dialogIdEquals && IVEquals && KeyEquals && rsaSignEquals && rsaToVerifyEquals1 && rsaToVerifyEquals2 && 
                session.VerificationData[0].UserID == sessionTest.VerificationData[0].UserID &&
                session.VerificationData[1].UserID == sessionTest.VerificationData[1].UserID);
        }
        [Test]
        public void ResaveLoadTest()
        {
            SessionIO io = new SessionIO();
            AesManaged aes = new AesManaged();
            RSACryptoServiceProvider rsaSign = new RSACryptoServiceProvider();
            List<UserVerificationData> data = new List<UserVerificationData>();

            RSACryptoServiceProvider rsaVerify = new RSACryptoServiceProvider();
            rsaVerify.PersistKeyInCsp = false;

            RSACryptoServiceProvider enotherRsaVerify = new RSACryptoServiceProvider();
            enotherRsaVerify.PersistKeyInCsp = false;
            data.Add(new UserVerificationData(34, rsaVerify));
            data.Add(new UserVerificationData(76, enotherRsaVerify));
            ClientClientEncryptedSession session = new ClientClientEncryptedSession(aes, 4218, rsaSign, data);
            io.Save(keyFileName, 76, session);

            
            AesManaged changedAes = new AesManaged();
            RSACryptoServiceProvider changedSign = new RSACryptoServiceProvider();
            RSACryptoServiceProvider changedVerify = new RSACryptoServiceProvider();
            ClientClientEncryptedSession changedSession = new ClientClientEncryptedSession(changedAes, 4218, changedSign, data);
            io.Save(keyFileName, 76, changedSession);

            ClientClientEncryptedSession sessionTest = io.LoadSession(4218, 76, keyFileName);
            bool dialogIdEquals = sessionTest.Dialog == session.Dialog;
            bool IVEquals = sessionTest.IV.SequenceEqual(changedSession.IV);
            bool KeyEquals = sessionTest.EncryptionKey.SequenceEqual(changedSession.EncryptionKey);
            bool rsaSignEquals = sessionTest.RsaToSign.ToXmlString(true).Equals(changedSession.RsaToSign.ToXmlString(true));
            string rsaVerTest1 = sessionTest.VerificationData[0].RsaToVerify.ToXmlString(false);
            string rsaVerTest2 = sessionTest.VerificationData[1].RsaToVerify.ToXmlString(false);
            string rsaVer1 = session.VerificationData[0].RsaToVerify.ToXmlString(false);
            string rsaVer2 = session.VerificationData[1].RsaToVerify.ToXmlString(false);
            bool rsaToVerifyEquals1 = rsaVerTest1.Equals(rsaVer1);
            bool rsaToVerifyEquals2 = rsaVerTest2.Equals(rsaVer2);
            Assert.That(dialogIdEquals && IVEquals && KeyEquals && rsaSignEquals && rsaToVerifyEquals1 &&
                rsaToVerifyEquals2 && session.VerificationData[0].UserID == sessionTest.VerificationData[0].UserID && 
                session.VerificationData[1].UserID == sessionTest.VerificationData[1].UserID);
        }
    }
}
