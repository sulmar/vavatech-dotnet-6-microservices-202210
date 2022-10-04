using Core.Domain;
using System.Xml.Serialization;

namespace CatalogService.Domain
{
    [XmlInclude(typeof(Product))]
    [XmlInclude(typeof(Service))]
    public abstract class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
    }

    public class Product : Item
    {        
        public Item? Alernate { get; set; }

        public string? Color { get; set; }

        public Product()
        {
            Status = ProductStatus.Draft;
        }
    }
    
    public class Service : Item
    {
        public TimeSpan Duration { get; set; }
    }

    public enum ProductStatus
    {
        Draft,
        Published,
        Removed
    }
}