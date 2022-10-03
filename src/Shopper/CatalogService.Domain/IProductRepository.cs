using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Domain
{
    public interface IProductRepository
    {
        IEnumerable<Product> Get();
        Product Get(int id);
        bool IsExists(int id);
        void Add(Product product);
        void Update(Product product);
        void Remove(int productId);
    }
}
