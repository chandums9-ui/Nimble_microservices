using Common.App.Contracts;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace Common.Infra.DataRepos
{


    public static class IEnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }

    public class ADOServices : IADOService
    {
        public async Task<int> ExecuteSqlNonQueryCommand(List<SqlParameter> sqlParams, string command, string connectionString)
        {
            int noOfRowsEffected = 0;
            SqlConnection _connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(command, _connection))
            {
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParams.ToArray());
                noOfRowsEffected = await cmd.ExecuteNonQueryAsync();
            }
            return noOfRowsEffected;

        }
        public async Task<string> ExecuteSqlScalarCommand(List<SqlParameter> sqlParams, string command, string connectionString)
        {
            object val = null;
            SqlConnection _connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(command, _connection))
            {
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParams.ToArray());
                val = await cmd.ExecuteScalarAsync();
            }
            return val == null ? "" : val.ToString();

        }

        public async Task<List<T>> ExecuteSqlCommand<T>(List<SqlParameter> sqlParams, string command, string connectionString)
        {
            List<T> list = new List<T>();

            SqlConnection _connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(command, _connection);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(sqlParams.ToArray());
            T obj = default(T);
            // Create a DataReader  
            using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                try
                {
                    while (await rdr.ReadAsync())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            try
                            {
                                if (!object.Equals(rdr[prop.Name], DBNull.Value))
                                {
                                    if (rdr[prop.Name].GetType().Name.ToString() == "Byte[]")
                                    {
                                        if (prop.CanWrite) prop.SetValue(obj, ByteArrayToString((Byte[])rdr[prop.Name]), null);
                                        continue;
                                    }
                                    if (prop.CanWrite) prop.SetValue(obj, rdr[prop.Name], null);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.GetType() == typeof(IndexOutOfRangeException))
                                {
                                    // if the result set doesn't have this value, intercept the exception
                                    // and set the property value to null / 0
                                    if (prop.CanWrite) prop.SetValue(obj, null, null);
                                }
                                else
                                    throw new Exception(Environment.NewLine + "Data Type Mapping Failed" + ">>" + prop.Name + "<<" + Environment.NewLine + ex.Message, ex);
                            }
                        }
                        list.Add(obj);
                    }
                    rdr.Close();
                }
                catch (Exception)
                {
                    rdr.Close();
                    throw;
                }
            }
            return list;
        }
        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return "0x" + hex.ToString();
        }
    }
}
