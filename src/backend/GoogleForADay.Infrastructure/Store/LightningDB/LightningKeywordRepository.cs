using System;
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
    public class LightningKeywordRepository : IKeyValueRepository<Keyword>
    {

        private LightningEnvironment _env;
        private LightningTransaction _txn;
        private LightningDatabase _db;

        private bool _alreadyInit;

        public LightningKeywordRepository()
        {
            if (_alreadyInit) return;

            Init();
            _alreadyInit = true;

        }
        public bool Init()
        {
            _env = new LightningEnvironment("storeLight");
            var dbName = "test";
            _env.MaxDatabases = 2;
            _env.Open();
            _txn = _env.BeginTransaction();
            _db = _txn.OpenDatabase(dbName, new DatabaseConfiguration { Flags = DatabaseOpenFlags.Create });

            return true;

        }

        public Keyword Get(object key)
        {
            if (!(key is string strKey))
                throw new ArgumentException("Invalid key type");

            var res = _txn.Get(_db, Encoding.UTF8.GetBytes(strKey));

            var strObj = Encoding.UTF8.GetString(res.value.CopyToNewArray());

            return JsonConvert.DeserializeObject<Keyword>(strObj);

        }

        public bool Upsert(Keyword entity)
        {
            var strObj = JsonConvert.SerializeObject(entity);

            var bytes = Encoding.UTF8.GetBytes(strObj);
            var res = _txn.Put(_db, Encoding.UTF8.GetBytes(entity.Term), bytes);

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
            throw new NotImplementedException();
        }


        public void SaveChanges()
        {
            _txn.Commit();
        }
    }
}
