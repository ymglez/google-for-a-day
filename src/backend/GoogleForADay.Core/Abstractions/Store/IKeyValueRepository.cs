using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoogleForADay.Core.Model;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Abstractions.Store
{
    public interface IKeyValueRepository<T> where T : Entity
    {
        bool Init();

        T Get(string key);

        bool Upsert(string key, T entity);

        bool Delete(object key);

        bool Clear();


        void SaveChanges();
    }
}
