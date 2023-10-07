using JWTAuthencation.Data;
using JWTAuthencation.Models;

namespace JWTAuthencation.Repositories
{
    public class UsersRepository
    {
        private readonly JWTAuthencationContext _context;
        public UsersRepository(JWTAuthencationContext context) 
        { 
            _context = context;
        }
        public IEnumerable<Users> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
