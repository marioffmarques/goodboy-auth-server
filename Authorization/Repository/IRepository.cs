using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets all Entities async.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="index">Index.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<IEnumerable<TEntity>> GetAllAsync(int? index, int? offset, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Gets Entities based on an Expression the async.
        /// </summary>
        /// <returns>The async.</returns>
        IEnumerable<TEntity> FindAsync(Func<TEntity, bool> clause, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Gets an Entity async
        /// </summary>
        /// <returns>The Entity.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="cancelationToken">CancelationToken</param>
        Task<TEntity> GetAsync(object id, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Add the specified entity.
        /// </summary>
        /// <returns>The added Entity</returns>
        /// <param name="entity">Entity.</param>
        /// <param name="cancelationToken">CancelationToken</param>
        void Add(TEntity entity, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Update the specified entity.
        /// </summary>
        /// <returns>Updated Entity</returns>
        /// <param name="entity">Entity.</param>
        /// <param name="cancelationToken">CancelationToken</param>
        void Update(TEntity entity, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Delete the specified entity.
        /// </summary>
        /// <returns>The deleted entity.</returns>
        /// <param name="entity">Entity.</param>
        /// <param name="cancelationToken">CancelationToken</param>
        void Delete(TEntity entity, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Count the specified cancelationToken.
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<int> Count(CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Count the specified clause and cancelationToken.
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="clause">Clause.</param>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<int> Count(Expression<Func<TEntity, bool>> clause, CancellationToken cancelationToken = default(CancellationToken));

        /// <summary>
        /// Commit the specified cancelationToken.
        /// </summary>
        /// <returns>The commit.</returns>
        /// <param name="cancelationToken">Cancelation token.</param>
        Task<int> Commit(CancellationToken cancelationToken = default(CancellationToken));
    }
}
