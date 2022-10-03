using Core.Domain;

namespace CatalogService.Domain
{
    public class Product : BaseEntity
    {        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }

        public Product()
        {
            Status = ProductStatus.Draft;
        }
    }

    public enum ProductStatus
    {
        Draft,
        Published,
        Removed
    }
}