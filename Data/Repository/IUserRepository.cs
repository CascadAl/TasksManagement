using System;
using Data.Entities;

namespace Data.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser GetUserById(int id);
    }
}