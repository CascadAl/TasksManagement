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
using System.Web.Configuration;

namespace Services.Classes
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository = null;
        private readonly ICommentRepository _commentRepository = null;
        private readonly IGroupRepository _groupRepository = null;
        private readonly IGroupService _groupService = null;
        private readonly IProfileService _profileService = null;

        public IssueService(IIssueRepository issueRepo, IProfileService profileService, ICommentRepository commentRepo, IGroupRepository groupRepo, IGroupService groupService)
        {
            _issueRepository = issueRepo;
            _commentRepository = commentRepo;
            _groupRepository = groupRepo;
            _groupService = groupService;
            _profileService = profileService;
        }

        private void Create (IssueViewModel viewModel)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            if (!_groupService.IsGroupOwner(viewModel.GroupId, userId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            var group = _groupService.Get(viewModel.GroupId);

            viewModel.IssueNumber = ++group.IssuesTotal;
            _groupRepository.Update(group);

            int issueId=_issueRepository.Add(viewModel.ToEntity());
            Comment comment = new Comment()
            {
                CreatedAt = DateTime.Now,
                IssueId = issueId,
                Text = string.IsNullOrWhiteSpace(viewModel.Text)? string.Empty : viewModel.Text,
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

            var issue = _issueRepository.Get(viewModel.Id.Value);
            var initialComment = issue.Comments.OrderBy(c => c.CreatedAt).First();

            issue.AssignedToUserId = viewModel.AssignedToUserId;
            issue.Title = viewModel.Title;
            initialComment.IsEdited = true;
            initialComment.LastEditedAt = DateTime.Now;
            initialComment.Text = string.IsNullOrWhiteSpace(viewModel.Text) ? string.Empty : viewModel.Text;
            
            return _issueRepository.Update(issue); 
        }
        public void CreateOrUpdate(IssueViewModel viewModel)
        {
            if (!viewModel.Id.HasValue)
                Create(viewModel);
            else
                Update(viewModel);
        }

        public IssueListViewModel GetAll(int groupId, string state)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();

            if (!_groupService.IsGroupParticipant(groupId, userId))
                throw new ArgumentException("You are not a member of this group");

            IQueryable<Issue> issues= _issueRepository.Get(g => g.GroupId == groupId).OrderByDescending(i => i.IssueNumber);

            //filtering
            if (state.Equals("open"))
                issues = issues.Where(i => i.ClosedAt == null);
            else
            if (state.Equals("closed"))
                issues = issues.Where(i => i.ClosedAt.HasValue);

            _profileService.AvatarFolder = WebConfigurationManager.AppSettings["AvatarFolder"];
            _profileService.DefaultAvatar = WebConfigurationManager.AppSettings["DefaultAvatar"];

            var filteredIssues = issues.ToList().Select(i => SetAvatarPath(i.ToViewModel()));
            return new IssueListViewModel() { Issues = filteredIssues, IsOwner = _groupService.IsGroupOwner(groupId, userId) };
        }

        public IssueListViewModel GetAssigned(string state)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            IQueryable<Issue> issues = _issueRepository.Get(i => i.AssignedToUserId.Value == userId).OrderBy(i => i.OpenedAt);

            //filtering
            if (state.Equals("open"))
                issues = issues.Where(i => i.ClosedAt == null);
            else
            if (state.Equals("closed"))
                issues = issues.Where(i => i.ClosedAt.HasValue);

            _profileService.AvatarFolder = WebConfigurationManager.AppSettings["AvatarFolder"];
            _profileService.DefaultAvatar = WebConfigurationManager.AppSettings["DefaultAvatar"];

            var filteredIssues = issues.ToList().Select(i => SetAvatarPath(i.ToViewModel()));
            return new IssueListViewModel() { Issues = filteredIssues };
        }


        private IssueViewModel SetAvatarPath(IssueViewModel viewModel)
        {
            if (viewModel.AssignedToUserId.HasValue)
            {
                var profileDto = _profileService.GetProfile(viewModel.AssignedToUserId.Value);
                viewModel.AsignedToUsername = profileDto.UserName;
                viewModel.AssignedToAvatarPath = profileDto.AvatarPath;
            }
            return viewModel;
        }

        public IssueViewModel Get (int issueId)
        {
            var issue = _issueRepository.Get(i => i.Id == issueId).SingleOrDefault();

            if (issue == null)
                throw new ArgumentException("No issue with this id was found.");

            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            if (!_groupService.IsGroupParticipant(issue.GroupId, userId))
                throw new ArgumentException("You are not a member of this group");

            return issue.ToViewModel();
        }

        public IssueDetailsViewModel Get(int groupId, int issueId)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            if (!_groupService.IsGroupParticipant(groupId, userId))
                throw new ArgumentException("You are not a member of this group");

            bool isOwner = _groupService.IsGroupOwner(groupId, userId);

            return _issueRepository.Get(i => i.Id == issueId).Single().ToDetailsViewModel(isOwner);
        }

        private void CreateComment(CommentViewModel viewModel)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();

            viewModel.CreatedAt = DateTime.Now;
            viewModel.IsEdited = false;
            viewModel.UserId = userId;

            _commentRepository.Add(viewModel.ToEntity());
        }

        private bool UpdateComment(CommentViewModel viewModel)
        {
            int currentUserId = HttpContext.Current.User.Identity.GetUserId<int>();
            var comment = _commentRepository.Get(c => c.Id == viewModel.Id.Value).SingleOrDefault();

            if (comment == null)
                throw new ArgumentException("No comment with this id was found");

            // если текущий пользователь не автор комментария и не владелец группы --> вернуть false
            if (!(comment.UserId == currentUserId || _groupService.IsGroupOwner(viewModel.GroupId, currentUserId)) )
                return false;

            comment.IsEdited = true;
            comment.LastEditedAt = DateTime.Now;
            comment.Text = viewModel.Text;

            return _commentRepository.Update(comment);
        }

        public void CreateOrUpdateComment(CommentViewModel viewModel)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            if (!_groupService.IsGroupParticipant(viewModel.GroupId, userId))
                throw new ArgumentException("You are not a member of this group");

            if (!viewModel.Id.HasValue)
                CreateComment(viewModel);
            else
                UpdateComment(viewModel);
                
        }

        public void RemoveComment(int commentId)
        {
            int currentUserId = HttpContext.Current.User.Identity.GetUserId<int>();
            var comment = _commentRepository.Get(c => c.Id == commentId).SingleOrDefault();

            if (comment == null)
                throw new ArgumentException("No comment with this id was found");

            // если текущий пользователь не автор комментария и не владелец группы --> кинить исключение
            if (!(comment.UserId == currentUserId || _groupService.IsGroupOwner(comment.Issue.GroupId, currentUserId)))
                throw new MemberAccessException("Only comment author or group owner can delete comments");

            if (comment.Text.Equals(string.Empty))
                throw new ArgumentException("You can't delete initial comment");

            _commentRepository.Remove(comment);
        }

        public bool Close(int issueId)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            var issue = _issueRepository.Get(i => i.Id == issueId).SingleOrDefault();

            if (issue == null)
                return false;

            if (!_groupService.IsGroupParticipant(issue.GroupId, userId))
                return false;

            if (issue.ClosedAt != null)
                return false;

            issue.ClosedAt = DateTime.Now;
            issue.ClosedByUserId = userId;

            return _issueRepository.Update(issue);
        }

        public bool Open(int issueId)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            var issue = _issueRepository.Get(i => i.Id == issueId).SingleOrDefault();

            if (issue == null)
                return false;

            if (!_groupService.IsGroupParticipant(issue.GroupId, userId))
                return false;

            if (issue.ClosedAt == null)
                return false;

            issue.ClosedAt = null;
            issue.ClosedByUserId = null;

            return _issueRepository.Update(issue);
        }

        public void Remove(int issueId)
        {
            int currentUserId = HttpContext.Current.User.Identity.GetUserId<int>();
            var issue = _issueRepository.Get(i => i.Id == issueId).SingleOrDefault();

            if (issue == null)
                throw new ArgumentException("No issue with this id was found.");

            // если текущий пользователь не автор  и не владелец группы --> кинуть исключение
            if (!(issue.OpenedByUserId == currentUserId || _groupService.IsGroupOwner(issue.GroupId, currentUserId)))
                throw new MemberAccessException("Only issue author or group owner can delete issues");

            _issueRepository.Remove(issue);
        }

        public int CountAssignedTasks()
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();
            return _issueRepository.Get(i => i.AssignedToUserId == userId && i.ClosedAt==null).Count();
        }
        public void Dispose()
        {
            _issueRepository.Dispose();
            _commentRepository.Dispose();
        }
    }
}
