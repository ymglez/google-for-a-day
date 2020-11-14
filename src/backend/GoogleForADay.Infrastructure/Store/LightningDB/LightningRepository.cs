using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FASTER.core;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model;
using GoogleForADay.Core.Model.Store;
using LightningDB;
using Newtonsoft.Json;

namespace GoogleForADay.Infrastructure.Store.LightningDB
{
    public class LightningRepository<T> : IKeyValueRepository<T> where T : Entity 
    {

        private LightningEnvironment _env;
        private LightningTransaction _txn;
        private LightningDatabase _db;

        private bool _alreadyInit;

        public LightningRepository()
        {
            if (_alreadyInit) return;

            Init();
            _alreadyInit = true;

        }
        public bool Init()
        {
            _env = new LightningEnvironment("store");
            var dbName = $"{typeof(T).Name}_db";
            _env.MaxDatabases = 2;
            _env.Open();
            _txn = _env.BeginTransaction();
            _db = _txn.OpenDatabase(dbName, new DatabaseConfiguration { Flags = DatabaseOpenFlags.Create });

            return true;

        }

        public T Get(string key)
        {
            var res = _txn.Get(_db, Encoding.UTF8.GetBytes(key));

            var strObj = Encoding.UTF8.GetString(res.value.CopyToNewArray());

            return JsonConvert.DeserializeObject<T>(strObj);

        }
        
        public bool Upsert(string key, T entity)
        {
            var strObj = JsonConvert.SerializeObject(entity);

            var bytes = Encoding.UTF8.GetBytes(strObj);
            var res = _txn.Put(_db, Encoding.UTF8.GetBytes(key), bytes);

            return res == MDBResultCode.Success;
        }

        public bool Delete(object key)
        {
            if (!(key is string strKey))
                throw new ArgumentException("Invalid key type");

            var res = _txn.Delete(_db, Encoding.UTF8.GetBytes(strKey));
            return res == MDBResultCode.Success;
        }

        public bool Clear()
        {
            var res = _db.Drop(_txn);

            return res == MDBResultCode.Success;
        }


        public void SaveChanges()
        {
            _txn.Commit();
        }
    }
}
