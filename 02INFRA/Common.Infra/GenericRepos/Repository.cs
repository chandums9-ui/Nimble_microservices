using Common.App.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Xml;
using static Amazon.S3.Util.S3EventNotification;

namespace Common.Infra.GenericRepos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private IAppDBContext context = null;
        private DbSet<T> Entities = null;
        public Repository(IAppDBContext ncontext)
        {
            this.context = ncontext;
            Entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Entities.ToListAsync();
        }
        public IQueryable<T> GetQueryable()
        {
            return Entities; 
        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> where)
        {
            return await Entities.Where(where).AsNoTrackingWithIdentityResolution().ToListAsync();
            //return await Entities.Where(where).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllByOffset(Expression<Func<T, bool>> where,int pageNo,int offset)
        {
            return await Entities.Where(where).Skip(pageNo* offset).Take(offset).AsNoTrackingWithIdentityResolution().ToListAsync();
            //return await Entities.Where(where).ToListAsync();
        }
        public async Task<T?> GetById(object id)
        {
            return await Entities.FindAsync(id);
        }
        public async Task Add(T obj)
        {
            await Entities.AddAsync(obj);

        }
        public async Task Update(T obj)
        {
            
            await Task.Run(() => Entities.Update(obj));

        }
        public async Task Delete(object id)
        {
            T? existing = await Entities.FindAsync(id);
            if (existing != null)
            {
                await Task.Run(() => Entities.Remove(existing));

            }

        }

        public async Task<int> Count()
        {
            return await Entities.CountAsync();

        }
        public async Task<int> Count(Expression<Func<T, bool>> where)
        {
            return await Entities.Where(where).CountAsync();

        }
        public async Task Delete(T obj)
        {

            if (obj != null)
            {
                await Task.Run(() => Entities.Remove(obj));
            }

        }
        public async Task ExecuteDelete(Expression<Func<T, bool>> where)
        {
            await Entities.Where(where).ExecuteDeleteAsync();

        }
        public async Task BulkInsert(List<T> list)
        {
            await Entities.AddRangeAsync(list);
        }

        public async Task BulkUpdate(List<T> list)
        {
            await Task.Run(() => Entities.UpdateRange(list));


        }
        public async Task BulkEnumUpdate(IEnumerable<T> list)
        {
            await Task.Run(() => Entities.UpdateRange(list));


        }
        public async Task BulkDelete(List<T> list)
        {
            await Task.Run(() => Entities.RemoveRange(list));

        }
        public async Task BulkEnumDelete(IEnumerable<T> list)
        {
            await Task.Run(() => Entities.RemoveRange(list));

        }
        public async Task ExecuteUpdate(Expression<Func<T, bool>> where, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            await Entities.Where(where).ExecuteUpdateAsync(setPropertyCalls);
        }

        public async Task InsertDetachedEntities(T entity) 
        {
            if (Entities.Entry(entity).State == EntityState.Detached)
                await Entities.AddAsync(entity);

        }
        public async Task InsertOrUpdateEntities(T entity)
        {
            var entry = Entities.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                Entities.Attach(entity);
                entry.State = EntityState.Modified;
            }
            else
            {
                await Entities.AddAsync(entity);
            }

        }
        public Task<int> SaveChanges()
        {
            return context.SaveChangesAsync();

        }
    }

    public class RepositoryBase : IRepositoryBase
    {
        public TSource DateTimeUTCConverstion<TSource>(TSource inputObj, bool IsConvertToUTC)
        {
            string clientTimeZone = "Eastern Standard Time";
            string typeName = string.Empty;

            try
            {
                if (inputObj != null)
                {
                    foreach (PropertyInfo pi in inputObj.GetType().GetProperties())
                    {
                        typeName = string.Empty;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            typeName = Nullable.GetUnderlyingType(pi.PropertyType).Name;

                        }
                        else { typeName = pi.PropertyType.Name; }

                        if (typeName.ToUpper() == "DATETIME")
                        {
                            //bool attValue = false;
                            //if (pi.GetCustomAttribute(typeof(UTCControlAttribute)) is UTCControlAttribute)
                            //    attValue = ((pi.GetCustomAttribute(typeof(UTCControlAttribute))) as UTCControlAttribute).UTCControl;
                            //else
                            //    attValue = true;

                            if (pi.GetValue(inputObj, null) != null)
                            {
                                try
                                {
                                    if (!IsConvertToUTC)
                                    {
                                        DateTime date = (DateTime)(pi.GetValue(inputObj, null));
                                        DateTime dt = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                                        // DateTime gstTime = TimeZoneInfo.ConvertTime(DateTime.Parse(pi.GetValue(inputObj, null).ToString(), System.Globalization.CultureInfo.InvariantCulture), TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone));
                                        if (pi.CanWrite) pi.SetValue(inputObj, TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone)), null);

                                    }
                                    else if (IsConvertToUTC)
                                    {
                                        // DateTime gstTime = TimeZoneInfo.ConvertTime(DateTime.Parse(pi.GetValue(inputObj, null).ToString(), System.Globalization.CultureInfo.InvariantCulture), TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone));
                                        DateTime date = (DateTime)(pi.GetValue(inputObj, null));
                                        DateTime dt = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                                        if (pi.CanWrite) pi.SetValue(inputObj, TimeZoneInfo.ConvertTimeToUtc(dt, TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone)), null);
                                    }
                                }
                                catch { }
                            }

                        }

                    }
                }
            }
            catch (Exception ex) { }
            return inputObj;
        }
    }
}
