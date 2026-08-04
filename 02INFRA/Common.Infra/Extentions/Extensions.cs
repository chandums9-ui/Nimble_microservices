using Common.Domain.DTO.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.Extentions
{
    public static class IDataReaderExtensions
    {
        #region DataReaderToList



        public static List<T> ToList<T>(this IDataReader rdr) 
        {
            var result = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite).ToList();
            var columnOrdinals = Enumerable.Range(0, rdr.FieldCount)
             .ToDictionary(rdr.GetName, i => i, StringComparer.OrdinalIgnoreCase);

            while (rdr.Read())
            {
                var entity =  Activator.CreateInstance<T>();

                foreach (var prop in properties)
                {
                    if (!columnOrdinals.TryGetValue(prop.Name, out var ordinal) || rdr.IsDBNull(ordinal))
                    {
                        prop.SetValue(entity, null);
                        continue;
                    }
                    var value = rdr[ordinal];
                    if (value is byte[] bytes)
                    {
                        prop.SetValue(entity, bytes);
                    }
                    else
                    {
                        prop.SetValue(entity, value);
                    }
                }

                result.Add(entity);
            }

            return result;
        }

    
        #endregion
    }
}
