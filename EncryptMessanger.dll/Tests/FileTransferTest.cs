using EncryptMessanger.dll.Encription;
using EncryptMessanger.dll.FileTransfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Tests
{
    [TestFixture]
    class FileTransferTest
    {
        private byte[] _signature;
        private ClientClientEncryptedSession _session; 
        
        private async void SendFileToServer(string fileName, ClientClientEncryptedSession session, IPEndPoint point)
        {
            FileSender sender = new FileSender();
            await sender.SendFileToServerAsync(fileName, session, point);
        }
        private async void ReceiveFileForServer(string fileName, IPEndPoint point)
        {
            FileReceiver reseiver = new FileReceiver();
            _signature = await reseiver.ReceiveFileForServerAsync(fileName, point);
        }
        private async void SendFileToClient(string fileName, IPEndPoint point, byte[] signature)
        {
            FileSender sender = new FileSender();
            await sender.SendFileToClientAsync(fileName, point, signature);
        }
        private async void ReceiveFileForClient(string fileName, ClientClientEncryptedSession session, IPEndPoint point)
        {
            FileReceiver reseiver = new FileReceiver();
            _signature =  await reseiver.ReceiveFileForClientAsync(fileName, point, session);
        }
        
        
        [Test]
        public  void ClientServerFileTransferTest()
        {
            string fileToSend = @"D:\test\TestSendFile.txt";
            IPEndPoint clientToServerPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11455);
            IPEndPoint serverToClientPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11459);
            long userId = 564;
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.PersistKeyInCsp = false;
            _session = new ClientClientEncryptedSession(new AesManaged(), 245, provider, new List<UserVerificationData>() {new UserVerificationData(userId, provider)});
            SendFileToServer(fileToSend, _session, clientToServerPoint);
            ReceiveFileForServer(@"D:\test\ReceivedTestSendFile.txt", clientToServerPoint);
            Thread.Sleep(8000);
            ReceiveFileForClient(@"D:\test\backClientFile.txt", _session, clientToServerPoint);
            SendFileToClient(@"D:\test\ReceivedTestSendFile.txt", clientToServerPoint, _signature);
            
            Thread.Sleep(9000);
            Assert.IsTrue(_session.VerifyFile(new FileStream(@"D:\test\backClientFile.txt", FileMode.Open), userId, _signature));
        }
    }
}
