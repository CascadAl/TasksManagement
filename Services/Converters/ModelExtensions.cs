using System;
using Data.Entities;
using Services.Models;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace Services.Converters
{
    public static class ModelExtensions
    {
        public static Group ToEntity (this GroupViewModel viewModel)
        {
            return new Group()
            {
                Id = viewModel.Id ?? 0,
                CreatedAt = DateTime.Now,
                Description = viewModel.Description,
                Title = viewModel.Title,
                IssuesTotal = 0
            };
        }

        public static GroupViewModel ToViewModel(this Group entity)
        {
            return new GroupViewModel()
            {
                Id = entity.Id,
                Description = entity.Description,
                Title = entity.Title
            };
        }

        public static GroupWRoleViewModel ToViewModel(this GroupMember entity)
        {
            return new GroupWRoleViewModel()
            {
                Id = entity.GroupId,
                Title = entity.Group.Title,
                Description = entity.Group.Description,
                Role = entity.Role.Name,
                JoinedAt=entity.JoinedAt
            };
        }

        public static MemberViewModel ToMemberViewModel(this GroupMember entity)
        {
            return new MemberViewModel()
            {
                UserId = entity.UserId,
                FullName=entity.User.UserProfile.FullName,
                Role=entity.Role.Name,
                RoleId=entity.RoleId,
                Username = entity.User.UserName,
                JoinedAt = entity.JoinedAt
            };
        }

        public static GroupRole ToGroupRole(this ApplicationRole entity)
        {
            return new GroupRole()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static AddMemberProfileViewModel ToAddMemberProfileViewModel (this ApplicationUser entity)
        {
            return new AddMemberProfileViewModel()
            {
                UserId = entity.Id,
                FullName = entity.UserProfile.FullName 
            };
        }

        public static GroupMember ToEntity(this GroupMemberViewModel viewModel)
        {
            return new GroupMember()
            {
                UserId = viewModel.UserId,
                GroupId = viewModel.GroupId,
                RoleId = viewModel.RoleId
            };
        }

        public static IssueViewModel ToViewModel(this Issue entity)
        {
            return new IssueViewModel()
            {
                Id = entity.Id,
                OpenedAt = entity.OpenedAt,
                GroupId = entity.GroupId,
                IssueNumber = entity.IssueNumber,
                Title = entity.Title,
                Text = entity.Id>0? entity.Comments.OrderBy(c=>c.CreatedAt).FirstOrDefault().Text : null,
                ClosedAt = entity.ClosedAt,
                AssignedToUserId = entity.AssignedToUserId,
                OpenedByUserId = entity.OpenedByUserId,
                OpenedByUser = entity.UserOpened.UserName,
                ClosedByUser = entity.UserClosed != null ? entity.UserClosed.UserName : null,
            };
        }

        public static IssueDetailsViewModel ToDetailsViewModel(this Issue entity, bool isOwner)
        {
            return new IssueDetailsViewModel()
            {
                Issue = entity.ToViewModel(),
                Comments = entity.Comments.Select(c => c.ToViewModel()),
                IsOwner=isOwner
            };
        }

        public static Issue ToEntity(this IssueViewModel viewModel)
        {
            return new Issue()
            {
                Id= viewModel.Id ?? 0,
                GroupId = viewModel.GroupId,
                OpenedAt = DateTime.Now,
                Title = viewModel.Title,
                IssueNumber = viewModel.IssueNumber,
                ClosedAt = viewModel.ClosedAt,
                AssignedToUserId = viewModel.AssignedToUserId,
                OpenedByUserId = HttpContext.Current.User.Identity.GetUserId<int>()
            };
        }

        public static CommentViewModel ToViewModel(this Comment entity)
        {
            return new CommentViewModel()
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                IsEdited = entity.IsEdited,
                LastEditedAt = entity.LastEditedAt,
                Text = entity.Text,
                Username = entity.User.UserName
            };
        }

        public static Comment ToEntity(this CommentViewModel viewModel)
        {
            return new Comment()
            {
                Id = viewModel.Id.HasValue ? viewModel.Id.Value : 0,
                CreatedAt = viewModel.CreatedAt,
                IsEdited = viewModel.IsEdited,
                IssueId = viewModel.IssueId,
                Text = viewModel.Text,
                LastEditedAt = viewModel.LastEditedAt,
                UserId = viewModel.UserId
            };
        }
    }
}