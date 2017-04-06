using System.Linq;
using Data.Entities;

namespace Data.Repository
{
    public class UserRepository: IUserRepository
    {
        private ApplicationDbContext _context = null;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id.Equals(id));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}