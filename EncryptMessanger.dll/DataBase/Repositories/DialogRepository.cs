using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.DataBase.Repositories
{
    public class DialogRepository : IRepositiry<Dialog>
    {
        private DataDataContext dc = new DataDataContext();
        public void Create(Dialog item)
        {
            dc.Dialog.InsertOnSubmit(item);
        }

        public void Delete(Dialog item)
        {
            dc.DialogMembers.DeleteAllOnSubmit(item.DialogMembers);
            dc.Message.DeleteAllOnSubmit(item.Message);
            dc.Dialog.DeleteOnSubmit(item);
        }

        public Dialog GetItem(int id)
        {
            return dc.Dialog.SingleOrDefault(d => d.id == id);
        }

        public IEnumerable<Dialog> GetItemList()
        {
            return dc.Dialog;
        }

        public void Save()
        {
            dc.SubmitChanges();
        }

        public void Update(Dialog item)
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", @"C:\Users\home\Documents\Visual Studio 2015\Projects\TesstDbProj\TesstDbProj\Database1.mdf");

        }
        public void AddMember(int id, DialogMembers member)
        {
            Dialog dialog = GetItem(id);
            dialog.DialogMembers.Add(member);
        }
    }
}
