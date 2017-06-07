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
        private string _avatarFolder;
        private string _defaultAvatar;

        public string AvatarFolder
        {
            get { return this._avatarFolder; }
            set { this._avatarFolder = value; }
        }

        public string DefaultAvatar
        {
            get { return this._defaultAvatar; }
            set { this._defaultAvatar = value; }
        }

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                    Path.Combine(AvatarFolder, user.UserProfile.AvatarName) :
                    DefaultAvatar
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
            IQueryable<ApplicationUser> users;

            if (query.Contains("@")) //search by username
            {
                users = _userRepository.Get(u => (u.UserName).Contains(query.Substring(1)));
            }
            else                    //search by fullname
            {
                users = _userRepository.Get(u => (u.UserProfile.FullName).Contains(query));
            }

            var viewModels = users.Take(7).ToList().Select(u => u.ToAddMemberProfileViewModel());

            return viewModels;
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
