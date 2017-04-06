﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public ICollection<GroupViewModel> GetAll(string userId)
        {
            var user=_userRepository.GetUserById(userId);

            return user.Groups.ToList().Select(g=>g.ToViewModel()).ToList();
        }

        public Group Get(int groupId)
        {
            throw new System.NotImplementedException();
        }

        public void CreateGroup(GroupViewModel newGroup, string userId)
        {
            var user = _userRepository.GetUserById(userId);

            user.Groups.Add(newGroup.ToEntity());
        }

        public bool UpdateGroup(GroupViewModel newGroup)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveGroup(int groupId)
        {
            throw new System.NotImplementedException();
        }
    }
}