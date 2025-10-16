using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradHub.Core;
using TradHub.Core.Entity;
using TradHub.Core.Repository_Contract;

namespace TradeHub.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity);
            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(key, repository);
            }
            return (IGenericRepository<TEntity>)_repositories[key];
        }
        public Task<int> CompleteAsync()
            => _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
            => _dbContext.DisposeAsync();
    }
}
