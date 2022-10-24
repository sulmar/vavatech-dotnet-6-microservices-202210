using AuthService.Domain;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure
{
    public class UserFaker : Faker<User>
    {
        public UserFaker(IPasswordHasher<User> passwordHasher)
        {
            UseSeed(0);
            StrictMode(true);
            RuleFor(p => p.Id, f => f.IndexFaker);
            RuleFor(p => p.Username, f => f.Person.UserName);
            RuleFor(p => p.FirstName, f => f.Person.FirstName);
            RuleFor(p => p.LastName, f=>f.Person.LastName);
            RuleFor(p => p.Email, f => f.Person.Email);
            RuleFor(p => p.Phone, f => f.Phone.PhoneNumber());
            RuleFor(p => p.Birthday, f => f.Date.Past(40));
            RuleFor(p => p.HashedPassword, (f,user) => passwordHasher.HashPassword(user, "12345"));
        }
    }
}