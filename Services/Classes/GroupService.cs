using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Data.Entities;
using Data.Repository;
using Services.Converters;
using Services.Interfaces;
using Services.Models;

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
            var groupId=_groupRepository.Add(newGroup.ToEntity());
            var role = _roleRepository.Get(r => r.Name.Equals("Owner")).Single();

            _groupMemberRepository.AddUserToGroup(groupId, userId, role.Id);
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

        private bool IsGroupOwner(int groupId, int userId)
        {
            var ownerRoleId = _roleRepository.Get(r => r.Name.Equals("Owner")).Select(r => r.Id).Single();

            var groupExists = _groupMemberRepository.Has(g => (g.GroupId == groupId && g.RoleId == ownerRoleId && g.UserId == userId));
            return groupExists;
        }

        private bool IsGroupParticipant(int groupId, int userId)
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

            _groupMemberRepository.AddUserToGroup(viewModel.GroupId, viewModel.UserId, viewModel.RoleId);
        }

        public void RemoveMember(RemoveMemberViewModel viewModel)
        {
            if (!IsGroupOwner(viewModel.GroupId, viewModel.UserId))
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            if (!IsGroupParticipant(viewModel.GroupId, viewModel.UserToRemove))
                throw new ArgumentException("User does not belong to this group");

            _groupMemberRepository.RemoveUserFromGroup(viewModel.GroupId, viewModel.UserToRemove);
        }
    }
}