using System.Linq;
using Data.Entities;

namespace Data.Repository
{
    public class UserRepository: BaseRepository<ApplicationUser>, IUserRepository
    {
        private ApplicationDbContext _context = null;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ApplicationUser GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id.Equals(id));
        }

    }
}