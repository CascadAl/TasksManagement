using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Data.Entities;
using Data.Repository;
using Services.Converters;
using Services.Interfaces;
using Services.Models;
using Microsoft.AspNet.Identity;
using System.Web;

namespace Services.Classes
{
    public class GroupService:IGroupService
    {
        private readonly IGroupRepository _groupRepository = null;
        private readonly IUserRepository _userRepository = null;
        private readonly IApplicationRoleRepository _roleRepository = null;
        private readonly IGroupMemberRepository _groupMemberRepository = null;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository, IApplicationRoleRepository roleRepository, IGroupMemberRepository groupMemberRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _groupMemberRepository = groupMemberRepository;
        }

        public void Dispose()
        {
            _groupRepository.Dispose();
            _userRepository.Dispose();
            _roleRepository.Dispose();
        }

        public ICollection<GroupWRoleViewModel> GetAll(int userId)
        {
            var groups=_groupMemberRepository.Get(g => g.UserId == userId).ToList();
            ICollection<GroupWRoleViewModel> groupViewModels = new List<GroupWRoleViewModel>();

            foreach (var group in groups)
            {
                groupViewModels.Add(group.ToViewModel());
            }
            return groupViewModels;
        }

        public Group Get(int groupId)
        {
            return _groupRepository.Get(groupId);
        }

        public GroupViewModel GetViewModel(int groupId, int userId)
        {
            if (!IsGroupParticipant(groupId, userId))
                throw new ArgumentException("You are not a member of this group");

            var group = _groupRepository.Get(groupId);

            return group.ToViewModel();
        }

        public void CreateGroup(GroupViewModel newGroup, int userId)
        {
            int groupId=_groupRepository.Add(newGroup.ToEntity());
            var role = _roleRepository.Get(r => r.Name.Equals("Owner")).Single();

            GroupMember groupMember = new GroupMember() { GroupId = groupId, UserId = userId, RoleId = role.Id, JoinedAt = DateTime.Now }; 
            _groupMemberRepository.AddUserToGroup(groupMember);
        }

        public void CreateOrUpdate(GroupViewModel groupViewModel, int userId)
        {
            if (!groupViewModel.Id.HasValue)
            {
                CreateGroup(groupViewModel, userId);
            }
            else
            {
                UpdateGroup(groupViewModel, userId);
            }
        }

        public bool UpdateGroup(GroupViewModel groupViewModel, int userId)
        {
            if (!groupViewModel.Id.HasValue)
                return false;

            if (!IsGroupOwner(groupViewModel.Id.Value, userId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            return _groupRepository.Update(groupViewModel.ToEntity());
        }

        public void RemoveGroup(int groupId, int userId)
        {
            if(!IsGroupOwner(groupId, userId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            _groupRepository.Remove(groupId);
        }

        public void LeaveGroup(int groupId)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();

            if (!IsGroupParticipant(groupId, userId))
                throw new ArgumentException("Wrong groupId or you are not a member of this group");

            var group = _groupRepository.Get(groupId);

            _groupMemberRepository.RemoveUserFromGroup(groupId, userId);

            if (group.Members.Count == 0)
                _groupRepository.Remove(groupId);
        }

        public bool IsGroupOwner(int groupId, int userId)
        {
            var ownerRoleId = _roleRepository.Get(r => r.Name.Equals("Owner")).Select(r => r.Id).Single();

            var groupExists = _groupMemberRepository.Has(g => (g.GroupId == groupId && g.RoleId == ownerRoleId && g.UserId == userId));
            return groupExists;
        }

        public bool IsGroupParticipant(int groupId, int userId)
        {
            return _groupMemberRepository.Has(g => (g.GroupId == groupId && g.UserId == userId));
        }

        public AddMemberViewModel GetMembers(int groupId, int userId)
        {
            if (!IsGroupParticipant(groupId, userId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            var members = _groupMemberRepository.Get(g => g.GroupId == groupId).ToList();
            var groupRoles = _roleRepository.Get(r => r.RoleType.Name.Equals("Group")).ToList();
            AddMemberViewModel viewModel = new AddMemberViewModel();


            foreach (var role in groupRoles)
            {
                viewModel.GroupRoles.Add(role.ToGroupRole());
            }

            foreach (var member in members)
            {
                viewModel.Members.Add(member.ToMemberViewModel());
            }
            viewModel.GroupId = groupId;
            viewModel.GroupTitle = members.First().Group.Title;

            return viewModel;
        }

        public void AddMember(GroupMemberViewModel viewModel)
        {
            if (IsGroupParticipant(viewModel.GroupId, viewModel.UserId))
                throw new ArgumentException("User is already in this group.");

            var entity = viewModel.ToEntity();
            entity.JoinedAt = DateTime.Now;

            _groupMemberRepository.AddUserToGroup(entity);
        }

        public bool RemoveMember(RemoveMemberViewModel viewModel)
        {
            int userId = HttpContext.Current.User.Identity.GetUserId<int>();

            if (!IsGroupOwner(viewModel.GroupId, userId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            if (!IsGroupParticipant(viewModel.GroupId, viewModel.UserToRemove))
                throw new ArgumentException("User does not belong to this group");

            if (IsGroupOwner(viewModel.GroupId, viewModel.UserToRemove))
            {
                int ownerRoleId = _roleRepository.Get(r => r.Name.Equals("Owner")).Select(r => r.Id).Single();
                int owners = _groupMemberRepository.Get(m => m.GroupId == viewModel.GroupId).Where(r => r.RoleId.Equals(ownerRoleId)).Count();

                if (owners > 1)
                {
                    _groupMemberRepository.RemoveUserFromGroup(viewModel.GroupId, viewModel.UserToRemove);
                    return true;
                }
                return false;
            }

            _groupMemberRepository.RemoveUserFromGroup(viewModel.GroupId, viewModel.UserToRemove);
            return true;
        }

        public void ChangeMemberRole(GroupMemberViewModel viewModel)
        {
            if (!IsGroupOwner(viewModel.GroupId, viewModel.CurrentUserId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            if (!IsGroupParticipant(viewModel.GroupId, viewModel.UserId))
                throw new ArgumentException("User does not belong to this group");

            _groupMemberRepository.Update(viewModel.ToEntity());
        }
    }
}