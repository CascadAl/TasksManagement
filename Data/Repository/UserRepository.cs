using System.Linq;
using System.Data.Entity;
using Data.Entities;
using System;

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

        public ApplicationUser GetAsNoTracking(int id)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}