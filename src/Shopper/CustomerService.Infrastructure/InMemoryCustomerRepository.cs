using Core.Infrastructure;
using CustomerService.Domain;

namespace CustomerService.Infrastructure
{
    public class InMemoryCustomerRepository : InMemoryEntityRepository<Customer>, ICustomerRepository
    {
        public InMemoryCustomerRepository()
        {
            _entities = new List<Customer>
            {
                new Customer { Id  = 1, FirstName = "John", LastName  = "Smith", Email = "john@domain.com"},
                new Customer { Id  = 2, FirstName = "Bob", LastName  = "Smith", Email = "bob@domain.com"},
                new Customer { Id  = 3, FirstName = "Kate", LastName  = "Smith", Email = "kate@domain.com"},
            };
        }
    }
}