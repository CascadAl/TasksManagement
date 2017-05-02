using Data.Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models;
using Services.Converters;
using Data.Entities;
using Microsoft.AspNet.Identity;
using System.Web;

namespace Services.Classes
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository = null;
        private readonly ICommentRepository _commentRepository = null;
        private readonly IGroupRepository _groupRepository = null;
        private readonly IGroupService _groupService = null;

        public IssueService(IIssueRepository issueRepo, ICommentRepository commentRepo, IGroupRepository groupRepo, IGroupService groupService)
        {
            _issueRepository = issueRepo;
            _commentRepository = commentRepo;
            _groupRepository = groupRepo;
            _groupService = groupService;
        }

        private void Create (IssueViewModel viewModel)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            var group = _groupService.Get(viewModel.GroupId);

            viewModel.IssueNumber = ++group.IssuesTotal;
            _groupRepository.Update(group);

            int issueId=_issueRepository.Add(viewModel.ToEntity());
            Comment comment = new Comment()
            {
                CreatedAt = DateTime.Now,
                IssueId = issueId,
                Text = viewModel.Text,
                UserId = userId,
                IsEdited = false
            };

            _commentRepository.Add(comment);
        }

        public bool Update (IssueViewModel viewModel)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            if (!_groupService.IsGroupOwner(viewModel.GroupId, userId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            return _issueRepository.Update(viewModel.ToEntity());
        }
        public void CreateOrUpdate(IssueViewModel viewModel)
        {
            if (!viewModel.Id.HasValue)
                Create(viewModel);
            else
                Update(viewModel);
        }

        public IEnumerable<IssueViewModel> Get(int groupId)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            if (!_groupService.IsGroupParticipant(groupId, userId))
                throw new ArgumentException("You are not a member of this group");

            return _issueRepository.Get(g => g.GroupId == groupId).ToList().Select(i=>i.ToViewModel());
        }

        public void RemoveIssue(int issueId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _issueRepository.Dispose();
            _commentRepository.Dispose();
        }
    }
}
