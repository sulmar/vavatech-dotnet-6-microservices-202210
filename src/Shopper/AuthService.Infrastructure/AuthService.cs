using AuthService.Domain;

namespace AuthService.Infrastructure
{
    public class MyAuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        public MyAuthService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool TryAuthorize(string username, string password, out User user)
        {
            user = userRepository.GetByUsername(username);

            return user != null && Verify(password, user.HashedPassword);
        }

        private static bool Verify(string password, string hashedPassword)
        {
            return hashedPassword == password;
        }
    }
}