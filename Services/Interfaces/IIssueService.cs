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
        IssueListViewModel GetAll(int groupId, string state);
        IssueListViewModel GetAssigned(string state);

        IssueViewModel Get(int issueId);

        IssueDetailsViewModel Get(int groupId, int issueId);

        void CreateOrUpdate(IssueViewModel viewModel);

        void CreateOrUpdateComment(CommentViewModel viewModel);

        bool Close(int issueId);

        bool Open(int issueId);

        void RemoveComment(int commentId);

        void Remove(int issueId);

        int CountAssignedTasks();
    }
}
