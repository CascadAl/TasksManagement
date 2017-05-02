using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IIssueService
    {
        IEnumerable<IssueViewModel> Get(int groupId);

        void CreateOrUpdate(IssueViewModel viewModel);

        void RemoveIssue(int issueId);
    }
}
