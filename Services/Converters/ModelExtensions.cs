using System;
using Data.Entities;
using Services.Models;

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
                Title = viewModel.Title
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
                FirstName=entity.User.UserProfile.FirstName,
                LastName=entity.User.UserProfile.LastName,
                Role=entity.Role.Name
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
                FullName = String.Format("{0} {1}", entity.UserProfile.FirstName, entity.UserProfile.LastName) 
            };
        }
    }
}