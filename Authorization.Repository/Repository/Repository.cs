using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly AuthorizationDbContext context;

        public Repository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public virtual void Add(TEntity entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            EntityState state = context.Entry(entity).State;

            if (state == EntityState.Detached)
            {
                context.Set<TEntity>().Attach(entity);
            }
            context.Set<TEntity>().AddAsync(entity, cancelationToken);
        }

        public void Delete(TEntity entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            var prop = entity.GetType().GetProperty("IsDeleted");
            if (prop.PropertyType == typeof(bool))
            {
                prop.SetValue(entity, true);
            }
            Update(entity, cancelationToken);
        }

        public IEnumerable<TEntity> FindAsync(Func<TEntity, bool> clause, CancellationToken cancelationToken = default(CancellationToken))
        {
            return context.Set<TEntity>().Where(clause);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(int? index, int? offset, CancellationToken cancelationToken = default(CancellationToken))
        {
            var query = context.Set<TEntity>().Where(e => !(bool)e.GetType().GetProperty("IsDeleted").GetValue(e)).AsNoTracking();

            if (index != null)
            {
                query = query.Skip(index.Value);
            }
            if (offset != null)
            {
                query = query.Take(offset.Value);
            }
            return await query.ToListAsync(cancelationToken);
        }

        public virtual Task<TEntity> GetAsync(object id, CancellationToken cancelationToken = default(CancellationToken))
        {
            return context.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity, CancellationToken cancelationToken = default(CancellationToken))
        {
            EntityState state = context.Entry(entity).State;

            if (state == EntityState.Detached)
            {
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                return;
            }
            state = EntityState.Modified;
        }

        public virtual Task<int> Count(CancellationToken cancelationToken = default(CancellationToken))
        {
            return context.Set<TEntity>().CountAsync(cancelationToken);
        }

        public virtual Task<int> Count(Expression<Func<TEntity, bool>> clause, CancellationToken cancelationToken = default(CancellationToken))
        {
            return context.Set<TEntity>().Where(clause).CountAsync(cancelationToken);
        }

        public virtual Task<int> Commit(CancellationToken cancelationToken = default(CancellationToken)) 
        {
            return context.SaveChangesAsync(cancelationToken);
        }
    }
}
