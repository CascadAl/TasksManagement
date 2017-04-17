using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Services.Models;
using Data.Repository;
using Data.Entities;

namespace Services.Classes
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository = null;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateAvatar(string path, string name)
        {
            throw new NotImplementedException();
        }

        public string GetAvatar(int userId)
        {
            throw new NotImplementedException();
        }

        public ProfileViewModel GetViewModel(int userId)
        {
            ApplicationUser user = _userRepository.GetUserById(userId);

            return new ProfileViewModel
            {
                Email = user.Email,
                FirstName = user.UserProfile.FirstName,
                LastName = user.UserProfile.LastName,
                AvatarPath = user.UserProfile.AvatarPath,
            };       
        }

        public void Save(ProfileViewModel profile)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepository.Dispose();;
        }
    }
}
