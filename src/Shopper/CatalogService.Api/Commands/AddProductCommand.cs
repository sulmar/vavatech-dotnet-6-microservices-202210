using CatalogService.Domain;
using MediatR;

namespace CatalogService.Api.Commands
{
    //public class AddProductNotification : INotification
    //{
    //    public Product Product { get; set; }
    //    public AddProductNotification(Product product)
    //    {
    //        this.Product = product;
    //    }
    //}


    // dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
    public record AddProductCommand(Product Product) : INotification; // Marker interface


   
}
