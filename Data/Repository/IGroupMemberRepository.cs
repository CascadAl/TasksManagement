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
        void AddUserToGroup(int groupId, int userId, int roleId);

        bool Has(Expression<Func<GroupMember, bool>> predicate);



    }
}
