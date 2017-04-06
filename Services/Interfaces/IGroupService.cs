using System;
using System.Collections.Generic;
using Data.Entities;
using Services.Models;

namespace Services.Interfaces
{
    public interface IGroupService: IDisposable
    {
        ICollection<GroupViewModel> GetAll(string useId);

        Group Get(int groupId);

        void CreateGroup(GroupViewModel newGroup, string userId);

        bool UpdateGroup(GroupViewModel newGroup);

        void RemoveGroup(int groupId);
    }
}