using System;
using System.IO;
using FASTER.core;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Infrastructure.Store.FASTER
{
    public class FasterKeywordRepository : FasterRepository, IKeyValueRepository<Keyword>
    {

        private FasterKV<string, Keyword> Db { get; set; }
        private IDevice Log { get; set; }
        private IDevice ObjLog { get; set; }

        private bool _alreadyInit;

        public FasterKeywordRepository()
        {
            if (_alreadyInit) return;

            Init();
            _alreadyInit = true;

        }
        public bool Init()
        {
            const long logSize = 1L << 20;
            Log = Devices.CreateLogDevice($@"{DataFolder}\keyword\data\keyword-log.log",true, recoverDevice: true);
            ObjLog = Devices.CreateLogDevice($@"{DataFolder}\keyword\data\keyword-log-obj.log",true, recoverDevice: true);
            Db = new FasterKV
                <string, Keyword>(
                    logSize,
                    new LogSettings
                    {
                        LogDevice = Log,
                        ObjectLogDevice = ObjLog,
                        MutableFraction = 0.3,
                        PageSizeBits = 15,
                        MemorySizeBits = 20
                    },
                    new CheckpointSettings
                    {
                        CheckpointDir = $"{DataFolder}/keyword/checkpoints"
                    },
                    new SerializerSettings<string, Keyword>
                    {
                        keySerializer = () => new Types.StringSerializer(),
                        valueSerializer = () => new Types.ObjectSerializer<Keyword>()
                    }
                );

            if (!Directory.Exists($"{DataFolder}/keyword/checkpoints")) return true;

            Console.WriteLine("call recover db");
            Db.Recover();
            return false;

        }

        public Keyword Get(object key)
        {
            using (var ss = Db.For(new Types.StoreFunctions<string,Keyword>())
                .NewSession<Types.StoreFunctions<string, Keyword>>())
            {
                if( !(key is string strKey))
                    throw new ArgumentException("Invalid key type");

                var input = new Types.StringInput();
                var output = new Types.StoreOutput<Keyword>();
                var ctx = new Types.StoreContext<Keyword>();
                ss.Read(ref strKey, ref input, ref output, ctx,1);
                return output.value;
            }
        }

        public bool Upsert(Keyword entity)
        {
            using (var ss = Db
                .For(new Types.StoreFunctions<string, Keyword>())
                .NewSession<Types.StoreFunctions<string, Keyword>>())
            {
      
                var ctx = new Types.StoreContext<Keyword>();
                var key = entity.Term;
                var status = ss.Upsert(ref key, ref entity, ctx, 1);
                
                return status == Status.OK;
            }
        }

        public bool Delete(object key)
        {
            using (var ss = Db
                .For(new Types.StoreFunctions<string, Keyword>())
                .NewSession<Types.StoreFunctions<string, Keyword>>())
            {

                if (!(key is string strKey))
                    throw new ArgumentException("Invalid key type");

                var ctx = new Types.StoreContext<Keyword>();
                var status = ss.Delete(ref strKey, ctx, 1);
                
                return status == Status.OK;
            }
        }

        public bool Clear()
        {
            throw new NotImplementedException();
        }


        public void SaveChanges()
        {
            Db.TakeFullCheckpoint(out Guid token);
            Db.CompleteCheckpointAsync().GetAwaiter().GetResult();
        }
    }
}
