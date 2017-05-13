using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebUI.Models;
using AutoMapper;
using Services.DataTransferObjects;
using System.IO;
using System.Web.Configuration;

namespace WebUI.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private IProfileService _profileService = null;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
            _profileService.AvatarFolder = WebConfigurationManager.AppSettings["AvatarFolder"];
            _profileService.DefaultAvatar = WebConfigurationManager.AppSettings["DefaultAvatar"];
        }

        // GET: Profile
        public ActionResult Index()
        {
            var profileDto = _profileService.GetProfile(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<ProfileDTO, ProfileDetailsViewModel>());
            var profile = Mapper.Map<ProfileDTO, ProfileDetailsViewModel>(profileDto);

            return View(profile);
        }

        // GET: Profile/Edit/
        public ActionResult Edit()
        {
            var profileDto = _profileService.GetProfile(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<ProfileDTO, EditProfileViewModel>());
            var profile = Mapper.Map<ProfileDTO, EditProfileViewModel>(profileDto);

            return View(profile);
        }

        // POST: Profile/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProfileViewModel model, string action)
        {
            if (!ModelState.IsValid)
                return View(model);

            Mapper.Initialize(cfg => cfg.CreateMap<EditProfileViewModel, ProfileDTO>());
            var profileDto = Mapper.Map<EditProfileViewModel, ProfileDTO>(model);

            if (action == "removeAvatar")
            {
                System.IO.File.Delete(Server.MapPath(model.AvatarPath));
                profileDto.AvatarPath = null;
            }
            else if (action == "update" && model.Avatar != null)
            {
                string uniqueName = User.Identity.GetUserId() + Path.GetExtension(model.Avatar.FileName);
                string path = Server.MapPath(_profileService.AvatarFolder);
                string fullPath = Path.Combine(path, uniqueName);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                model.Avatar.SaveAs(fullPath);
                profileDto.AvatarPath = uniqueName;
            }

            _profileService.Save(profileDto);

            return RedirectToAction("Edit");
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult UserMenuButton()
        {
            var profileDto = _profileService.GetProfile(User.Identity.GetUserId<int>());
            Mapper.Initialize(cfg => cfg.CreateMap<ProfileDTO, ProfileDetailsViewModel>());
            var profile = Mapper.Map<ProfileDTO, ProfileDetailsViewModel>(profileDto);

            return PartialView("_UserMenuButton", profile);
        }
    }
}
