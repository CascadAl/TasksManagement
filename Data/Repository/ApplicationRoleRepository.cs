using Data.Entities;

namespace Data.Repository
{
    public class ApplicationRoleRepository : BaseRepository<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}