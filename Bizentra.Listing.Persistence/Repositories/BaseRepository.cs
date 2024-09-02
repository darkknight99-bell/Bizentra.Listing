using Bizentra.Listing.Application.Persistence.Helpers;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Bizentra.Listing.Domain.Attributes;
using Newtonsoft.Json;

namespace Bizentra.Listing.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly BizentraListingDbContext _context;
        public BaseRepository(BizentraListingDbContext dbContext)
        {
            _context = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<long> CountAll() => await _context.Set<T>().AsNoTracking().CountAsync();

        public async Task<long> CountWhere(Expression<Func<T, bool>> predicate) => await _context.Set<T>().AsNoTracking().CountAsync(predicate);

        public async Task<T> CreateAsync(T entity)
        {
            var returnItem = await _context.Set<T>().AddAsync(entity);
            return returnItem.Entity;
        }

        public async Task<bool> CreateMultipleAsync(IEnumerable<T> entity)
        {
            try
            {
                await _context.Set<T>().AddRangeAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                //log error info to entral logging system.
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            try
            {
                T entity = await ReadSingleAsync(Id);
                _context.Set<T>().Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                //log error info to entral logging system.
                return false;
            }
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false)
        {
            T data;
            if (ChildObjectNamesToInclude != null && ChildObjectNamesToInclude.Count() > 0)
            {
                var queryable = _context.Set<T>().AsQueryable();
                foreach (var item in ChildObjectNamesToInclude)
                {
                    queryable = queryable.Include(item);
                }
                if (WithTracking)
                    data = await queryable.FirstOrDefaultAsync(predicate);
                else
                    data = await queryable.AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            else
            {
                if (WithTracking)
                    data = await _context.Set<T>().FirstOrDefaultAsync(predicate);
                else
                    data = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
            }
            return data;
        }

        public async Task<T> SingleOrDefault(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false)
        {
            T data;
            if (ChildObjectNamesToInclude != null && ChildObjectNamesToInclude.Count() > 0)
            {
                var queryable = _context.Set<T>().AsQueryable();
                foreach (var item in ChildObjectNamesToInclude)
                {
                    queryable = queryable.Include(item);
                }
                if (WithTracking)
                    data = await queryable.SingleOrDefaultAsync(predicate);
                else
                    data = await queryable.AsNoTracking().SingleOrDefaultAsync(predicate);
            }
            else
            {
                if (WithTracking)
                    data = await _context.Set<T>().SingleOrDefaultAsync(predicate);
                else
                    data = await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
            }
            return data;
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false)
        {
            List<T> data = new List<T>();
            if (ChildObjectNamesToInclude != null && ChildObjectNamesToInclude.Count() > 0)
            {
                var queryable = _context.Set<T>().AsQueryable();
                foreach (var item in ChildObjectNamesToInclude)
                {
                    queryable = queryable.Include(item);
                }
                if (WithTracking)
                    data = await queryable.Where(predicate).ToListAsync();
                else
                    data = await queryable.Where(predicate).AsNoTracking().ToListAsync();
            }
            else
            {
                if (WithTracking)
                    data = await _context.Set<T>().Where(predicate).ToListAsync();
                else
                    data = await _context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
            }
            return data;
        }

        public IQueryable<T> GetWhereQueryable(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false)
        {
            IQueryable<T> data;
            if (ChildObjectNamesToInclude != null && ChildObjectNamesToInclude.Count() > 0)
            {
                var queryable = _context.Set<T>().AsQueryable();
                foreach (var item in ChildObjectNamesToInclude)
                {
                    queryable = queryable.Include(item);
                }
                if (WithTracking)
                    data = queryable.Where(predicate);
                else
                    data = queryable.Where(predicate).AsNoTracking();
            }
            else
            {
                if (WithTracking)
                    data = _context.Set<T>().Where(predicate);
                else
                    data = _context.Set<T>().Where(predicate).AsNoTracking();
            }
            return data;
        }

        public async Task<decimal?> Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> sumpredicate)
        {


            return (await _context.Set<T>().Where(predicate).SumAsync(sumpredicate));

        }

        public async Task<Paginated<T>> GetWherePaginated(PaginatedQuery<T> query)
        {
            int start = (query.Page - 1) * query.PageSize;
            List<T> data = new List<T>();
            if (query.Order == Order.Ascending)
            {
                if (query.ChildObjectNamesToInclude != null && query.ChildObjectNamesToInclude.Count() > 0)
                {
                    var queryable = _context.Set<T>().AsQueryable();
                    foreach (var item in query.ChildObjectNamesToInclude)
                    {
                        queryable = queryable.Include(item);
                    }
                    queryable = queryable.Where(query.predicate).OrderBy(x => x.Id).AsNoTracking().Skip(start).Take(query.PageSize);
                    data = await queryable.ToListAsync();
                }
                else
                {
                    data = await _context.Set<T>().Where(query.predicate).OrderBy(x => x.Id).AsNoTracking().Skip(start).Take(query.PageSize).ToListAsync();
                }
            }
            else
            {
                if (query.ChildObjectNamesToInclude != null && query.ChildObjectNamesToInclude.Count() > 0)
                {
                    var queryable = _context.Set<T>().AsQueryable();
                    foreach (var item in query.ChildObjectNamesToInclude)
                    {
                        queryable = queryable.Include(item);
                    }
                    data = await queryable.Where(query.predicate).OrderByDescending(x => x.Id).AsNoTracking().Skip(start).Take(query.PageSize).ToListAsync();
                }
                else
                {
                    data = await _context.Set<T>().Where(query.predicate).OrderByDescending(x => x.Id).AsNoTracking().Skip(start).Take(query.PageSize).ToListAsync();
                }
            }
            var count = await CountWhere(query.predicate);
            return new Paginated<T> { Page = query.Page, PageSize = query.PageSize, Data = data, TotalCount = count };
        }

        public async Task<IEnumerable<T>> ReadAllAsync(bool WithTracking = false)
        {
            IEnumerable<T> returnItem;
            if (WithTracking)
                returnItem = await _context.Set<T>().ToListAsync<T>();
            else
                returnItem = await _context.Set<T>().AsNoTracking().ToListAsync<T>();
            return returnItem;
        }

        public async Task<IEnumerable<T>> ReadByIdsAsync(Guid[] Id, bool WithTracking = false)
        {
            IEnumerable<T> returnItem;
            if (WithTracking)
                returnItem = await _context.Set<T>().Where(a => Id.Contains(a.Id)).ToListAsync<T>();
            else
                returnItem = await _context.Set<T>().AsNoTracking().Where(a => Id.Contains(a.Id)).ToListAsync<T>();
            return returnItem;
        }

        /// <summary>
        /// Return a single search result satisfing the search Id
        /// </summary>
        /// <param name="Id">The Search id</param>
        /// <param name="WithTracking">This indicates if the search if for update or not hence the object with the tracked.</param>
        /// <returns></returns>
        public async Task<T> ReadSingleAsync(Guid Id, bool WithTracking = true, string include = null)
        {
            T returnItem;
            if (WithTracking)
                if (!string.IsNullOrEmpty(include))
                    returnItem = await _context.Set<T>().Include(include).SingleOrDefaultAsync(x => x.Id == Id);
                else
                    returnItem = await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == Id);
            else
            {
                if (!string.IsNullOrEmpty(include))
                    returnItem = await _context.Set<T>().Include(include).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Id);
                else
                    returnItem = await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == Id);
            }
            return returnItem;
        }

        public async Task UpdateAsync(T entity)
        {
            var prevValue = await ReadSingleAsync(entity.Id, false);
            CompareUpdate(prevValue, entity);

            _context.Entry(prevValue).OriginalValues.SetValues(prevValue);
            _context.Entry(prevValue).CurrentValues.SetValues(entity);

            await Task.Run(() => _context.Entry(prevValue).State = EntityState.Modified);

        }

        private void CompareUpdate(T Prevalue, T NewValue)
        {
            var properties = GenerateListOfProperties(GetProperties);
            var notupdatableppty = GetNotUpdatable();
            properties.ForEach(property =>
            {
                var entityvalue = NewValue.GetType().GetProperty(property).GetValue(NewValue, null);
                var oldvalue = Prevalue.GetType().GetProperty(property).GetValue(Prevalue, null);
                if ((property.Equals("Id") || notupdatableppty.Contains(property) || entityvalue == null))
                {
                    if (oldvalue != entityvalue)
                        NewValue.GetType().GetProperty(property).SetValue(NewValue, oldvalue);
                }
            });
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        private string[] GetNotUpdatable()
        {
            return (from prop in GetProperties
                    let attributes = prop.GetCustomAttributes(typeof(NotUpdatableAttribute), false)
                    where attributes.Length > 0 && (attributes[0] as NotUpdatableAttribute)?.IgnoreUpdate == true
                    select prop.Name).ToArray();
        }

        public async Task UpdateMultipleAsync(IEnumerable<T> entity)
        {
            foreach (var item in entity)
            {
                await Task.Run(() => _context.Entry(item).State = EntityState.Modified);
            }
        }

        public async Task<T> CreateApprovedAsync(string T)
        {
            var entity = JsonConvert.DeserializeObject<T>(T);
            return await CreateAsync(entity);
        }

        public async Task UpdateApprovedAsync(string T)
        {
            var entity = JsonConvert.DeserializeObject<T>(T);
            await UpdateAsync(entity);
        }

        public async Task<bool> DeleteApprovedAsync(string Id)
        {
            return await DeleteAsync(Guid.Parse(Id));
        }
    }
}
