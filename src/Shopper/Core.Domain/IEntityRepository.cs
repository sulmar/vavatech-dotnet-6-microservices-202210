using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public interface IEntityRepository<TEntity>
        where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Get();
        TEntity Get(int id);
        bool IsExists(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(int id);
    }
}
