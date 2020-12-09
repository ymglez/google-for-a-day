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
        
        private LightningDatabase _db;

        private readonly bool _alreadyInit;

        public string DataPath { get; set; } = "store";

        public LightningRepository()
        {
            if (_alreadyInit) return;

            Init();
            _alreadyInit = true;

        }

        public bool Init()
        {
            _env = new LightningEnvironment(DataPath);
            var dbName = $"{typeof(T).Name}_db";
            _env.MaxDatabases = 2;
            _env.MapSize = int.MaxValue;
            _env.Open();
            using (var txn = _env.BeginTransaction())
            {
                _db = txn.OpenDatabase(dbName, new DatabaseConfiguration { Flags = DatabaseOpenFlags.Create });
                txn.Commit();
            }
            
            return true;

        }




        public T Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            using (var transaction = _env.BeginTransaction(TransactionBeginFlags.ReadOnly))
            {
                var res = transaction.Get(_db, Encoding.UTF8.GetBytes(key));

                var strObj = Encoding.UTF8.GetString(res.value.CopyToNewArray());

                return JsonConvert.DeserializeObject<T>(strObj);
            }
        }

        public bool Upsert(string key, T entity)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            using (var transaction = _env.BeginTransaction())
            {
                entity.Modified = DateTime.Now;
                var strObj = JsonConvert.SerializeObject(entity);
                var bytes = Encoding.UTF8.GetBytes(strObj);
                transaction.Put(_db, Encoding.UTF8.GetBytes(key), bytes);
                var code = transaction.Commit();

                return code == MDBResultCode.Success;
            }
        }

        public bool BulkUpsert(IDictionary<string, T> entities)
        {
            using (var transaction = _env.BeginTransaction())
            {
                foreach (var (key, entity) in entities)
                {
                    entity.Modified = DateTime.Now;
                    var strObj = JsonConvert.SerializeObject(entity);

                    var bytes = Encoding.UTF8.GetBytes(strObj);
                    transaction.Put(_db, Encoding.UTF8.GetBytes(key), bytes);
                }

                try
                {
                    var code = transaction.Commit();
                    return code == MDBResultCode.Success;
                }
                catch 
                {
                    return false;
                }
            }
        }

        public bool Delete(object key)
        {
            if (!(key is string strKey) || string.IsNullOrEmpty(strKey))
                throw new ArgumentException("Invalid key type");
            using (var transaction = _env.BeginTransaction())
            {
                var res = transaction.Delete(_db, Encoding.UTF8.GetBytes(strKey));
                transaction.Commit();
                return res == MDBResultCode.Success;
            }
        }

        public bool Clear()
        {
            try
            {
                _db.Dispose();
                _env.Dispose();

                if (Directory.Exists(DataPath))
                    Directory.Delete(DataPath, true);

                return Init();
            }
            catch
            {
                return false;
            }
            
            
        }

        public void SaveChanges()
        {
            //no needed
        }

        public long Count()
        {
            using (var transaction = _env.BeginTransaction(TransactionBeginFlags.ReadOnly))
            {
                return transaction.GetEntriesCount(_db);
            }
        }
    }
}
