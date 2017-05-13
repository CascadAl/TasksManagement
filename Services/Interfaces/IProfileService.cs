using Services.DataTransferObjects;
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
        string  AvatarFolder { get; set; }

        string DefaultAvatar { get; set; }

        ProfileDTO GetProfile(int userId);

        void Save(ProfileDTO profile);

        IEnumerable<AddMemberProfileViewModel> GetProfileData(string query);
    }
}
