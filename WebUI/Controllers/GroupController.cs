using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNet.Identity;
using Services.Interfaces;
using Services.Models;

namespace WebUI.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        private IGroupService _groupService = null;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        // GET: Groups
        public ActionResult Index()
        {
            
            var groups = _groupService.GetAll(User.Identity.GetUserId());
            return View(groups);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(GroupViewModel newGroup)
        {
            if (!ModelState.IsValid)
                return View(newGroup);

            _groupService.CreateGroup(newGroup, User.Identity.GetUserId());

            return RedirectToAction("Index");
        }
    }
}