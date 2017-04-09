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

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public void Dispose()
        {
            _groupRepository.Dispose();
            _userRepository.Dispose();
        }

        public ICollection<GroupViewModel> GetAll(int userId)
        {
            var user=_userRepository.GetUserById(userId);

            return user.Groups.ToList().Select(g=>g.ToViewModel()).ToList();
        }

        public Group Get(int groupId)
        {
            return _groupRepository.Get(groupId);
        }

        public GroupViewModel GetViewModel(int groupId, int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var group= user.Groups.FirstOrDefault(g => g.Id == groupId);

            if (group == null)
                throw new ArgumentException("Wrong groupId or group does not belong to you");

            return group.ToViewModel();
        }

        public void CreateGroup(GroupViewModel newGroup, int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var group = newGroup.ToEntity();

            user.Groups.Add(group);
            _userRepository.Update(user);
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
            var user = _userRepository.GetAsNoTracking(userId);
            
            var groupExists = user.Groups.Any(g => g.Id == groupId);
            return groupExists;
        }
    }
}