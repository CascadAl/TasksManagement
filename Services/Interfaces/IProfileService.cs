using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProfileService : IDisposable
    {
        ProfileViewModel GetViewModel(int userId);

        void Save(ProfileViewModel profile);

        string GetAvatar(int userId);

        void CreateAvatar(string path, string name);
    }
}
