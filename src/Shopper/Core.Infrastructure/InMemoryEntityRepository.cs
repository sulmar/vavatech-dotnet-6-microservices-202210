using Core.Domain;

namespace Core.Infrastructure
{
    public abstract class InMemoryEntityRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected IList<TEntity> _entities;
        
        public virtual void Add(TEntity entity)
        {
            int id = _entities.Max(p => p.Id);

            entity.Id = ++id;

            _entities.Add(entity);
        }

        public virtual IEnumerable<TEntity> Get()
        {
            return _entities;
        }

        public virtual TEntity Get(int id)
        {
            return _entities.SingleOrDefault(p => p.Id == id);
        }

        public virtual bool IsExists(int id)
        {
            return _entities.Any(p => p.Id == id);
        }

        public virtual void Remove(int id)
        {
            _entities.Remove(Get(id));
        }

        public virtual void Update(TEntity entity)
        {
            var id = entity.Id;

            Remove(id);

            Add(entity);

            entity.Id = id;
        }
    }
}