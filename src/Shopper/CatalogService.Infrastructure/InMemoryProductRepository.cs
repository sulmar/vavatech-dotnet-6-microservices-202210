﻿using CatalogService.Domain;

namespace CatalogService.Infrastructure
{

    public class InMemoryProductRepository : IProductRepository
    {
        private readonly ICollection<Product> _products;

        public InMemoryProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1"  },
                new Product { Id = 2, Name = "Product 2" },
                new Product { Id = 3, Name = "Product 3" },
            };
        }

        public void Add(Product product)
        {
            int id = _products.Max(p=>p.Id);

            product.Id = ++id;

            _products.Add(product);
        }

        public IEnumerable<Product> Get()
        {
            return _products;
        }

        public Product Get(int id)
        {
            return _products.SingleOrDefault(p => p.Id == id);
        }

        public bool IsExists(int id)
        {
            return _products.Any(p => p.Id == id);
        }

        public void Remove(int productId)
        {
            _products.Remove(Get(productId));
        }

        public void Update(Product product)
        {
            var id = product.Id;

            Remove(id);

            Add(product);

            product.Id = id;
        }
    }
}