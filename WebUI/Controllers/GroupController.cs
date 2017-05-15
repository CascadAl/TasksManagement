using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNet.Identity;
using Services.Converters;
using Services.Interfaces;
using Services.Models;
using System.Net;

namespace WebUI.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        private IGroupService _groupService = null;
        private IProfileService _profileService = null;

        public GroupController(IGroupService groupService, IProfileService profileService)
        {
            _groupService = groupService;
            _profileService = profileService;
        }

        public ActionResult Index()
        {
            var groups = _groupService.GetAll(User.Identity.GetUserId<int>());
            return View(groups);
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Action = "Create";
            return View("GroupForm", new GroupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(GroupViewModel newGroup)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Edit";
                return View("GroupForm", newGroup);
            }

            _groupService.CreateOrUpdate(newGroup, User.Identity.GetUserId<int>());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GroupViewModel viewModel = _groupService.GetViewModel(id, User.Identity.GetUserId<int>());
            ViewBag.Action = "Edit";

            return View("GroupForm", viewModel);
        }

        [HttpPost]
        public ActionResult Leave(int id)
        {
            ViewBag.Leaved=_groupService.LeaveGroup(id);
            var groups = _groupService.GetAll(User.Identity.GetUserId<int>());

            return View("Index", groups);
        }

        [HttpGet]
        public ActionResult Remove(int id)
        {
            _groupService.RemoveGroup(id, User.Identity.GetUserId<int>());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Members(int id)
        {
            var memberModels = _groupService.GetMembers(id, User.Identity.GetUserId<int>());
            if (HttpContext.Request.IsAjaxRequest()) return Json(memberModels, JsonRequestBehavior.AllowGet);

            return View(memberModels);
        }

        [HttpGet]
        public ActionResult GetUsers(string query)
        {
            var users = _profileService.GetProfileData(query);
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(GroupMemberViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            viewModel.CurrentUserId = User.Identity.GetUserId<int>();
            _groupService.AddMember(viewModel);

            return RedirectToAction("Members", new { id = viewModel.GroupId});
        }

        [HttpPost]
        public ActionResult RemoveMember(RemoveMemberViewModel viewModel)
        {
            int userId = User.Identity.GetUserId<int>();
            viewModel.UserId = userId;
            bool isDeleted=_groupService.RemoveMember(viewModel);

            if (isDeleted)
                return Json(userId);

            return new HttpStatusCodeResult(HttpStatusCode.NotModified);
        }

        [HttpPost]
        public ActionResult ChangeMemberRole(GroupMemberViewModel viewModel)
        {
            viewModel.CurrentUserId = User.Identity.GetUserId<int>();
            _groupService.ChangeMemberRole(viewModel);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}