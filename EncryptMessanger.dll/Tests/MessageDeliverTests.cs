using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Net.Sockets;
using System.Net;
using EncryptMessanger.dll.Messages;

namespace EncryptMessanger.dll.Tests
{
    [TestFixture]
    public class MessageDeliverTests
    {
        
        //MemoryStream ms;
        [SetUp]
        public void Init()
        {
            //ms = new MemoryStream();
        }
        [Test]
        public void ReadWriteTest()
        {
            IPAddress licalIP = System.Net.IPAddress.Parse("127.0.0.1");
            IPEndPoint listenerPoint = new IPEndPoint(licalIP, 44444);
            TcpListener localListener = new TcpListener(listenerPoint);
            TcpClient localClient = new TcpClient(new IPEndPoint(licalIP, 44445));
            localListener.Start();
            localClient.Connect(listenerPoint);

            TcpClient asseptedClient = localListener.AcceptTcpClient();

            MessageWriter mw = new MessageWriter(localClient.GetStream());
            MessageReader mr = new MessageReader(asseptedClient.GetStream());

            mw.WriteMessage(new TextMessage(1, 1, Encoding.UTF8.GetBytes("new text")));
            TextMessage message = mr.ReadNext() as TextMessage;
            Assert.That(message.From == 1 && message.Dialog == 1
                && message.Text.Equals("new text"));

            localClient.Close();
            asseptedClient.Close();            
            localListener.Stop();
        }
    }
}
