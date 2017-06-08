using Data.Repository;
using Services.Interfaces;
using Services.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Services.Models;
using Services.Converters;
using Data.Entities;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Configuration;

namespace Services.Classes
{
    public class BaseService : IBaseService
    {
        private readonly IUserRepository _userRepository = null;
        
        public BaseService(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public void CheckAccessPermission(ProfileDTO profile)
        {
            var user = _userRepository.Get(profile.Id);
            if (user.AccessFailedCount > 5)
            {
                user.AccessFailedCount = user.AccessFailedCount++;
                _userRepository.SaveChanges();
            }

            var profileService = new ProfileService(_userRepository);
            profileService.Save(profile);
        }
    }
}
