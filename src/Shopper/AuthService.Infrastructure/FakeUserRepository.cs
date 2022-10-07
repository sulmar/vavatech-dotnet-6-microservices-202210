using AuthService.Domain;
using Bogus;
using Core.Infrastructure;

namespace AuthService.Infrastructure
{

    public class FakeUserRepository : InMemoryEntityRepository<User>, IUserRepository
    {
        public FakeUserRepository(Faker<User> faker)
        {
            _entities = faker.Generate(100);
        }

        public User GetByUsername(string username)
        {
            return _entities.SingleOrDefault(e => e.Username == username);
        }
    }
}