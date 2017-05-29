using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using EncryptMessanger.dll.SendibleData;

namespace EncryptMessanger.dll.Tests
{
    [TestFixture]
    class DialogSendibleInfoTest
    {
        private bool CompareDialogInfo(DialogSendibleInfo dialogInfo)
        {
            return dialogInfo.DialogId == 12 && dialogInfo.DialogName.Equals("ThisIsMyDialogName") &&
                dialogInfo.CreationTime.Equals(new DateTime(2017, 2, 21)) && dialogInfo.EncryptMessages == true &&
                dialogInfo.SignMessages == false && dialogInfo.MembersId[0] == 3 && dialogInfo.MembersId[1] == 2 &&
                dialogInfo.MembersId[2] == 54 && dialogInfo.MembersId[3] == 78
                && dialogInfo.CreatorId == 1;
        }
        [Test]
        public void TransformationTest()
        {
            DialogSendibleInfo dialogInfo = new DialogSendibleInfo(12, "ThisIsMyDialogName", 
                new DateTime(2017, 2, 21), true, false, 1, new long[] { 3, 2, 54, 78});
            byte[] infoBytes = dialogInfo.ToByte();
            dialogInfo.FillFromBytes(infoBytes);
            Assert.That(CompareDialogInfo(dialogInfo));
        }
    }
}
