using CatalogService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Infrastructure
{
    public class FakeMessageService : IMessageService
    {
        public void Send(Product product)
        {
            Console.WriteLine(product);
        }
    }
}
