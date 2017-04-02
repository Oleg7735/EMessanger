﻿using EncryptMessanger.dll.SendibleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.Messages
{
    public class DialogsResponceMessage:Message
    {
        public void AddDialogInfo(DialogSendibleInfo dialogInfo)
        {
            AddAtribute(new MessageAtribute( Atribute.DialogInfo, dialogInfo.ToByte()));
        }
        public List<DialogSendibleInfo> GetDialogsInfo()
        {
            List<DialogSendibleInfo> dialogsInfo = new List<DialogSendibleInfo>();
            
            foreach(MessageAtribute atribute in _atributes)
            {
                if(atribute.Name == Atribute.DialogInfo)
                {
                    dialogsInfo.Add(new DialogSendibleInfo(atribute.Value));
                }
            }
            return dialogsInfo;
        }
    }
}
