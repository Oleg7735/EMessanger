using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptMessanger.dll.DataBase.Repositories
{
    interface IRepositiry<T> where T : class
    {
        IEnumerable<T> GetItemList(); // получение всех объектов
        T GetItem(int id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(T item); // удаление объекта по id
        void Save();//сохранить изменения
    }
}
