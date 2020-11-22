using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoogleForADay.Core.Model;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Abstractions.Store
{
    /// <summary>
    /// Persistence/retrieve functions based on key-value store model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IKeyValueRepository<T> where T : Entity
    {
        bool Init();

        T Get(string key);

        bool Upsert(string key, T entity);

        bool BulkUpsert(IDictionary<string,T> entities);

        bool Delete(object key);

        bool Clear();


        void SaveChanges();
    }
}
