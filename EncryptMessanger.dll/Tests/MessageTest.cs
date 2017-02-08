using EncryptMessanger.dll.Messages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Tests
{
    [TestFixture]
    public class MessageTest
    {
        [Test]
        public void TextMessageText()
        {
            TextMessage message = new TextMessage("user1","user2","new text");
            byte[] b = message.ToByte();
            TextMessage recreatetMessage = Message.CreateMessage(b) as TextMessage;
            Assert.That(message.To.Equals(recreatetMessage.To)&& message.From.Equals(recreatetMessage.From)&& message.Text.Equals(recreatetMessage.Text));

        }
        [Test]
        public void TextMessageToStringTest()
        {
            TextMessage message = new TextMessage("user1", "user2", "new text");
            byte[] sign = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 };
            message.AddSignature(sign);
            string s = message.ToString();

            Assert.That(s.Equals("Text message from user1 to user2 text = new text Signature = "+ Encoding.UTF8.GetString(message.GetSignature())));

        }
        [Test]
        public void TextMessageConstrTest()
        {
            TextMessage message = new TextMessage("user1", "user2", "new text");
            byte[] sign = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            message.AddSignature(sign);
            Assert.That(message.Atributes.Count == 4&& message.Type == MessageType.TextMessage&& 
                Encoding.UTF8.GetString(message.GetAttribute(Atribute.From)).Equals("user1")&&
                Encoding.UTF8.GetString(message.GetAttribute(Atribute.To)).Equals("user2")&&
                Encoding.UTF8.GetString(message.GetAttribute(Atribute.Text)).Equals("new text"));

        }
    }
}
