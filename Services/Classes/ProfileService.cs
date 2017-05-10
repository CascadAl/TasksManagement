using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Services.Models;
using Data.Repository;
using Data.Entities;
using Services.DataTransferObjects;
using System.Web.Configuration;
using System.IO;
using Services.Converters;

namespace Services.Classes
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository = null;
        private readonly string _avatarFolder = null;
        private readonly string _defaultAvatar = null;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _avatarFolder = WebConfigurationManager.AppSettings["AvatarFolder"];
            _defaultAvatar = WebConfigurationManager.AppSettings["DefaultAvatar"];
        }

        public ProfileDTO GetProfile(int userId)
        {
            ApplicationUser user = _userRepository.GetUserById(userId);

            return new ProfileDTO
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.UserProfile.FullName,
                AvatarPath = user.UserProfile.AvatarName != null ?
                    Path.Combine(_avatarFolder, user.UserProfile.AvatarName) :
                    _defaultAvatar
            };       
        }

        public void Save(ProfileDTO profile)
        {
            ApplicationUser user = _userRepository.GetUserById(profile.Id);
            user.UserName = profile.UserName;
            user.UserProfile.FullName = profile.FullName;
            user.UserProfile.AvatarName= profile.AvatarPath;

            _userRepository.SaveChanges();
        }

        public IEnumerable<AddMemberProfileViewModel> GetProfileData(string query)
        {
            var users = _userRepository.Get(u => (u.UserProfile.FullName).Contains(query)).Take(10).ToList();
            var viewModels = users.Select(u => u.ToAddMemberProfileViewModel());

            return viewModels;
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
