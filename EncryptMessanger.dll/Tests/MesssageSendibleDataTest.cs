using EncryptMessanger.dll.SendibleData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Tests
{
    [TestFixture]
    class MesssageSendibleDataTest
    {
        [Test]
        public void  MessageSendibleDataConvertionTest()
        {
            byte[] text = new byte[10] { 4, 65, 2, 6, 86, 3, 5, 6, 53, 4 };
            byte[] sign = new byte[10] { 4, 3, 2, 6, 86, 3, 7, 6, 23, 4 };
            MessageSendibleInfo info = new MessageSendibleInfo(15, 946, new DateTime(2017, 4, 14), text);
            info.Signature = sign;
            byte[] byteInfo = info.ToByte();
            MessageSendibleInfo enotherInfo = new MessageSendibleInfo();
            enotherInfo.FillFromBytes(byteInfo);
            Assert.That(enotherInfo.AuthorId == info.AuthorId && enotherInfo.DialogId == info.DialogId &&
                enotherInfo.SendTime.Equals(info.SendTime) && enotherInfo.Text.SequenceEqual(info.Text) && 
                enotherInfo.Signature.SequenceEqual(info.Signature));
        }
    }
}
