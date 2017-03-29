using Data;
using Data.Entities;

namespace Data.Repository
{
    public class TestRepository: BaseRepository<Test>, ITestRepository
    {
        public TestRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
