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

        public ICollection<GroupViewModel> GetAll(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var viewModels = user.GroupMembers.Select(e => e.Group.ToViewModel()).ToList();
            return viewModels;
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
            //var user = _userRepository.GetAsNoTracking(userId);
            var ownerRoleId = _roleRepository.Get(r => r.Name.Equals("Owner")).Select(r => r.Id).Single();

            var groupExists = _groupMemberRepository.Has(g => (g.GroupId == groupId && g.RoleId == ownerRoleId && g.UserId == userId));
            return groupExists;
        }

        private bool IsGroupParticipant(int groupId, int userId)
        {
            return _groupMemberRepository.Has(g => (g.GroupId == groupId && g.UserId == userId));
        }
    }
}