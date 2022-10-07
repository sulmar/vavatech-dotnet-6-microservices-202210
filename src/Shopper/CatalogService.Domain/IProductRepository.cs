using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Domain
{
    public interface IProductRepository : IEntityRepository<Product>
    {
       IEnumerable<Product> GetActive();
    }
}
