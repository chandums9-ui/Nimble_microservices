using Dashboard.Infra.DBCon;
using Dashboard.App.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Common.App.Contracts;
using Dashboard.Domain.DataModel;
using Common.Infra.GenericRepos;

namespace Dashboard.Infra.DataRepos
{
    public class UnitOfWork : IUnitOfWorkDashboard, IDisposable
    {
        public DashboardCustomizationContext context;
        #region Ctor

        public UnitOfWork(DashboardCustomizationContext cont)
        {
            this.context = cont;
           
        }

        #endregion Ctor

        #region Fields
        public IRepository<Widgets> widgets;
        public IRepository<WidgetUserSettings> widgetUserSettings;
        public IRepository<WidgetCustomization> widgetCustomizations;
        public IRepository<WidgetFormula> widgetFormulas;
        public IRepository<WidgetFormulaSettings> widgetFormulaSettings;
        public IRepository<WidgetPrivilegeDetails> widgetPrivilegeDetails;
        public IRepository<WidgetPrivileges> widgetPrivileges;
        public IRepository<WidgetFormulaDetails> widgetFormulaDetails;  

        #endregion Fields

        public IRepository<Widgets> Widgets
        {
            get
            {
                if (this.widgets == null)
                {
                    this.widgets = new Repository<Widgets>(context);
                }
                return widgets;
            }
        }
        public IRepository<WidgetUserSettings> WidgetUserSettings
        {
            get
            {
                if (this.widgetUserSettings == null)
                {
                    this.widgetUserSettings = new Repository<WidgetUserSettings>(context);
                }
                return widgetUserSettings;
            }
        }
        public IRepository<WidgetCustomization> WidgetCustomizations
        {
            get
            {
                if (this.widgetCustomizations == null)
                {
                    this.widgetCustomizations = new Repository<WidgetCustomization>(context);
                }
                return widgetCustomizations;
            }
        }
        public IRepository<WidgetFormula> WidgetFormulas
        {
            get
            {
                if (this.widgetFormulas == null)
                {
                    this.widgetFormulas = new Repository<WidgetFormula>(context);
                }
                return widgetFormulas;
            }
        }
        public IRepository<WidgetFormulaSettings> WidgetFormulaSettings
        {
            get
            {
                if (this.widgetFormulaSettings == null)
                {
                    this.widgetFormulaSettings = new Repository<WidgetFormulaSettings>(context);
                }
                return widgetFormulaSettings;
            }
        }
        public IRepository<WidgetPrivilegeDetails> WidgetPrivilegeDetails
        {
            get
            {
                if(this.widgetPrivilegeDetails==null)
                {
                    this.widgetPrivilegeDetails = new Repository<WidgetPrivilegeDetails>(context);
                }
                return widgetPrivilegeDetails;
            }
        }
        public IRepository<WidgetPrivileges> WidgetPrivileges
        {
            get
            {
                if(this.widgetPrivileges == null)
                {
                    this.widgetPrivileges = new Repository<WidgetPrivileges>(context);
                }
                return widgetPrivileges;
            }
        }

        public IRepository<WidgetFormulaDetails> WidgetFormulaDetails
        {
            get
            {
                if (this.widgetFormulaDetails == null)
                {
                    this.widgetFormulaDetails = new Repository<WidgetFormulaDetails>(context);  
                }
                return widgetFormulaDetails;    
            }
        }

        #region CUD
        public void ClearChangeTracker()
        {
            context.ChangeTracker.Clear();
        }
        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public async Task<int> SaveWithTransactionAsync()
        {
            using var ctxtTransaction = context.Database.BeginTransaction();
            try
            {
                int rowsEffected = await context.SaveChangesAsync();
                await ctxtTransaction.CommitAsync();
                return rowsEffected;
            }
            catch (Exception ex) { await ctxtTransaction.RollbackAsync(); return 0; }
        }

        public int SaveWithTransaction()
        {
            using var ctxtTransaction = context.Database.BeginTransaction();
            try
            {
                int rowsEffected = context.SaveChanges();
                ctxtTransaction.Commit();
                return rowsEffected;
            }
            catch (Exception ex) { ctxtTransaction.Rollback(); return 0; }

        }
#endregion

        #region SqlCommands
        public async Task<string> ExecuteSqlScalarCommand(List<SqlParameter> sqlParams, string command)
        {

            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = 360;
            cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] paramList = sqlParams.ToArray();
            cmd.Parameters.AddRange(paramList);
            // Open database connection  
            await context.Database.OpenConnectionAsync();


            // Create a DataReader  
            object val = await cmd.ExecuteScalarAsync();
            return val == null ? "" : val.ToString();
        }
        public async Task<int> ExecuteSqlNonQueryCommand(List<SqlParameter> sqlParams, string command)
        {

            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = 360;
            cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] paramList = sqlParams.ToArray();
            cmd.Parameters.AddRange(paramList);
            // Open database connection  
            await context.Database.OpenConnectionAsync();


            // Create a DataReader  
            int val = await cmd.ExecuteNonQueryAsync();
            return val;
        }
        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return "0x" + hex.ToString();
        }
        public async Task<List<T>> ExecuteSqlCommand<T>(List<SqlParameter> sqlParams, string command)
        {
            List<T> list = new List<T>();


            DbCommand cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = 360;
            cmd.CommandText = command;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] paramList = sqlParams.ToArray();
            cmd.Parameters.AddRange(paramList);
            // Open database connection  
            // OpenConnection(cmd);
            await context.Database.OpenConnectionAsync();
            T obj = default(T);
            // Create a DataReader  
            using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
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
                catch (Exception ex)
                {
                    rdr.Close();
                    throw;
                }
            }


            return list;
        }

        #endregion
        
        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
