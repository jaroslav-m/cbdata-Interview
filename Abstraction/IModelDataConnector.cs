using CbData.Interview.Abstraction.Model;

namespace CbData.Interview.Abstraction
{
    /// <summary>
    /// Represents a connector to the persistent storage of model data entities.
    /// </summary>
    public interface IModelDataConnector
    {
        /// <summary>
        /// Inserts a new <see cref="IEntity"/>.
        /// </summary>
        /// <param name="orders">The collection of <see cref="Order"/> to be processed.</param>
        public void Insert<T>(params T[] entities) where T : class, IEntity;

        /// <summary>
        /// Updates an existing <see cref="IEntity"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(params T[] entities) where T : class, IEntity;

        /// <summary>
        /// Deletes an existing <see cref="IEntity"/>.
        /// </summary>
        /// <param name="orders">The collection of <see cref="Order"/> to be deleted.</param>
        public void Delete<T>(params T[] entities) where T : IEntity;

        /// <summary>
        /// Gets a <see cref="IEntity"/> query.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Query<T>() where T : class, IEntity;

        /// <summary>
        /// Commits the changes to the database.
        /// </summary>
        public void Commit();
    }
}
