using Data.Entities;

namespace Data.Repository
{
    public class GroupRepository: BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}