using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Domain
{
    public interface ICustomerRepository : IEntityRepository<Customer>
    {

    }
}
