using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.DataBase.Repositories
{
    public class UserRepository : IRepositiry<Users>
    {
        private DataDataContext dc = new DataDataContext();

        public void Create(Users item)
        {
            dc.Users.InsertOnSubmit(item);
        }

        public void Delete(Users user)
        {
            dc.DialogMembers.DeleteAllOnSubmit(user.DialogMembers);
            dc.Message.DeleteAllOnSubmit(user.Message);
            dc.Users.DeleteOnSubmit(user);

        }

        public Users GetItem(int id)
        {
            return dc.Users.SingleOrDefault(u => u.id == id);
        }

        public IEnumerable<Users> GetItemList()
        {
            return dc.Users;
        }

        public void Save()
        {
            dc.SubmitChanges();
        }

        public void Update(Users item)
        {
            Users updatedUser = GetItem(item.id);

            updatedUser.ip = item.ip;

            updatedUser.login = item.login;
            updatedUser.hash = item.hash;

        }
        public Users GetUserByLogin(string login)
        {
            return dc.Users.SingleOrDefault(u => u.login.Equals(login));
        }
    }
}
