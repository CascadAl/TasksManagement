using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class IssueRepository : BaseRepository<Issue>, IIssueRepository
    {
        private readonly ApplicationDbContext _context = null;
        public IssueRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void AssignToNoone(int groupId, int userId)
        {
            var assignedIssues = _context.Issues.Where(i => i.AssignedToUserId == userId && i.GroupId == groupId).ToList();

            foreach (var issue in assignedIssues)
            {
                issue.AssignedToUserId = null;
                this.Update(issue);
            }
        }
    }
}
