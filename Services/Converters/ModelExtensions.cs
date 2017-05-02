using System;
using Data.Entities;
using Services.Models;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity;

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
                Role = entity.Role.Name
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
                ClosedAt = entity.ClosedAt,
                AssignedToUserId = entity.AssignedToUserId,
                //AssignedToUserId = entity.AssignedToUserId.HasValue? entity.AssignedToUserId.Value : 0,
                OpenedByUserId = entity.OpenedByUserId,
                OpenedByUser = entity.UserOpened.UserName,
                ClosedByUser = entity.UserClosed != null ? entity.UserClosed.UserName : null
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
    }
}