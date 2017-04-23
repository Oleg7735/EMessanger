using EncryptMessanger.dll.SendibleData;
using EncryptMessangerClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessangerClient.extensions
{
    static class DialogsObservableCollectionExtension
    {
        //public static void AddFromDialogInfo(this ObservableCollection<Dialog> dialogs, DialogSendibleInfo info)
        //{
        //    Dialog dialog = new Dialog(info.DialogName, info.DialogId, info.EncryptMessages, info.SignMessages, info.MembersId);
            
        //    if(!dialogs.Contains(dialog))
        //    {
        //        dialogs.Add(dialog);
        //    }
        //}
        public static Dialog GetById(this ObservableCollection<Dialog> dialogs, long dialogId)
        {
            foreach(Dialog dialog in dialogs)
            {
                if(dialog.DialogId == dialogId)
                {
                    return dialog;
                }
            }
            throw new ArgumentException("Не найден диалог с идентификатором "+dialogId.ToString());
        }
    }
}
