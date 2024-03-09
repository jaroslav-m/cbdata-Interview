using CbData.Interview.Abstraction;
using CbData.Interview.Common;
using Microsoft.EntityFrameworkCore;

namespace CbData.Interview.ModelDataConnector
{
    /// <summary>
    /// Represents connector that stores data to the database.
    /// </summary>
    public class DbModelDataConnector : IModelDataConnector
    {
        private readonly ModelDataContext _dbContext;

        /// <summary/>
        public DbModelDataConnector(ModelDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                // TODO: log error
            }
            finally
            {
                _dbContext.ChangeTracker.Clear();
            }
        }

        /// <inheritdoc/>
        public void Delete<T>(params T[] entities) where T : IEntity
        {
            foreach (var entity in entities)
                _dbContext.Remove(entity);
        }

        /// <inheritdoc/>
        public void Insert<T>(params T[] entities) where T : class, IEntity
        {
            foreach (var entity in entities)
                _dbContext.Set<T>().Add(entity);
        }

        /// <inheritdoc/>
        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        /// <inheritdoc/>
        public void Update<T>(params T[] entities) where T : class, IEntity
        {
            foreach (var entity in entities)
                _dbContext.Update(entity);
        }
    }
}
