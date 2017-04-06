using System;
using Data.Entities;

namespace Data.Repository
{
    public interface IUserRepository : IDisposable
    {
        ApplicationUser GetUserById(string id);
    }
}