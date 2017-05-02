using System;
using System.Collections.Generic;
using Data.Entities;
using Services.Models;

namespace Services.Interfaces
{
    public interface IGroupService: IDisposable
    {
        ICollection<GroupWRoleViewModel> GetAll(int useId);

        Group Get(int groupId);

        GroupViewModel GetViewModel(int groupId, int userId);

        void CreateGroup(GroupViewModel newGroup, int userId);

        void CreateOrUpdate(GroupViewModel groupViewModel, int userId);

        bool UpdateGroup(GroupViewModel groupViewModel, int userId);

        void RemoveGroup(int groupId, int userId);

        AddMemberViewModel GetMembers(int groupId, int userId);

        void AddMember(GroupMemberViewModel viewModel);

        void RemoveMember(RemoveMemberViewModel viewModel);

        void ChangeMemberRole(GroupMemberViewModel viewModel);

        bool IsGroupOwner(int groupId, int userId);

        bool IsGroupParticipant(int groupId, int userId);
    }
}