using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Domain.DTO.App
{
    //public class KeyValuePairs
    //{
    //    public int ID { get; set; }
    //    public string Name { get; set; }
    //}
    //public class KeyValuePairsLong
    //{
    //    public long ID { get; set; }
    //    public string Name { get; set; }
    //}

    public class KeyValuePairObject<T1, T2>
    {
        public KeyValuePairObject()
        {

        }
        public KeyValuePairObject(T1 key, T2 value)
        {
            this.Key = key;
            this.Value = value;
        }
        public T1 Key { get; set; }
        public T2 Value { get; set; }
    }

    public class KeyValuePairObject<T1, T2, T3>
    {
        public KeyValuePairObject()
        {

        }
        public KeyValuePairObject(T1 key, T2 value, T3 priority)
        {
            this.Key = key;
            this.Value = value;
            this.Priority = priority;
        }
        public T1 Key { get; set; }
        public T2 Value { get; set; }
        public T3 Priority { get; set; }
    }
}
