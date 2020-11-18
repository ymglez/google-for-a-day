using System;
using System.Text;
using FASTER.core;
using Newtonsoft.Json;

namespace GoogleForADay.Infrastructure.Store.FASTER
{
    public static class Types
    {
        public class StringSerializer : BinaryObjectSerializer<string>
        {
            public override void Deserialize(out string obj)
            {
                var bytesr = new byte[4];
                reader.Read(bytesr, 0, 4);
                var sizet = BitConverter.ToInt32(bytesr);
                var bytes = new byte[sizet];
                reader.Read(bytes, 0, sizet);
                obj = Encoding.UTF8.GetString(bytes);
            }

            public override void Serialize(ref string obj)
            {
                var bytes = Encoding.UTF8.GetBytes(obj);
                var len = BitConverter.GetBytes(bytes.Length);
                writer.Write(len);
                writer.Write(bytes);
            }
        }


        public class ObjectSerializer<T> : BinaryObjectSerializer<T>
        {

            public override void Deserialize(out T obj)
            {
                var bytesr = new byte[4];
                reader.Read(bytesr, 0, 4);
                var sizet = BitConverter.ToInt32(bytesr);
                var bytes = new byte[sizet];
                reader.Read(bytes, 0, sizet);
                var strObj = Encoding.UTF8.GetString(bytes);

                obj = JsonConvert.DeserializeObject<T>(strObj);
            }

            public override void Serialize(ref T obj)
            {
                var strObj = JsonConvert.SerializeObject(obj);

                var bytes = Encoding.UTF8.GetBytes(strObj);
                var len = BitConverter.GetBytes(bytes.Length);
                writer.Write(len);
                writer.Write(bytes);
            }
        }


        public class StringInput
        {
            public string value;
        }

        public class StoreOutput<T>
        {
            public T value;
        }

        public class StoreContext<T>
        {
            private Status _status;
            private StoreOutput<T> _output;

            internal void Populate(ref Status status, ref StoreOutput<T> output)
            {
                _status = status;
                _output = output;
            }

            internal void FinalizeRead(ref Status status, ref StoreOutput<T> output)
            {
                status = _status;
                output = _output;
            }
        }


        public sealed class StoreFunctions<TKey, TVal> : FunctionsBase<TKey, TVal, StringInput, StoreOutput<TVal>, StoreContext<TVal>>
        {
            public override void SingleReader(ref TKey key, ref StringInput input, ref TVal value, ref StoreOutput<TVal> dst)
            {
                dst.value = value;
            }

            public override void ConcurrentReader(ref TKey key, ref StringInput input, ref TVal value, ref StoreOutput<TVal> dst)
            {
                dst.value = value;
            }

            public override void ReadCompletionCallback(ref TKey key, ref StringInput input, ref StoreOutput<TVal> output, StoreContext<TVal> ctx,
                Status status)
            {
                ctx.Populate(ref status, ref output);
            }

           
        }


    }
}
