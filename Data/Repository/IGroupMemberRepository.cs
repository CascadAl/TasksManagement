using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Repository
{
    public interface IGroupMemberRepository
    {
        void AddUserToGroup(GroupMember entity);

        void RemoveUserFromGroup(int groupId, int userId);

        bool Has(Expression<Func<GroupMember, bool>> predicate);

        IQueryable<GroupMember> Get(Expression<Func<GroupMember, bool>> predicate);

        bool Update(GroupMember entity);

        string GetRole(int groupId, int userId);

        bool IsInGroup(int groupId, int userId);

        int CountOwners(int groupId);
    }
}
