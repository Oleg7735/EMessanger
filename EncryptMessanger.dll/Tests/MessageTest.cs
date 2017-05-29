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
            TextMessage message = new TextMessage(1,1,"new text");
            byte[] b = message.ToByte();
            TextMessage recreatetMessage = Message.CreateMessage(b) as TextMessage;
            Assert.That(message.Dialog == recreatetMessage.Dialog && message.From.Equals(recreatetMessage.From)&& message.Text.Equals(recreatetMessage.Text));

        }
        [Test]
        public void TextMessageToStringTest()
        {
            TextMessage message = new TextMessage(1, 2, "new text");
            byte[] sign = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 };
            message.AddSignature(sign);
            string s = message.ToString();

            Assert.That(s.Equals("Text message from user1 to dialog2 text = new text Signature = "+ Encoding.UTF8.GetString(message.GetSignature())));

        }
        [Test]
        public void TextMessageConstrTest()
        {
            TextMessage message = new TextMessage(1, 2, "new text");
            byte[] sign = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            message.AddSignature(sign);
            Assert.That(message.Atributes.Count == 5 && message.Type == MessageType.TextMessage &&
                message.From == 1 &&
                message.Dialog == 2 &&
                message.Text.Equals("new text"));
            

        }
    }
}
