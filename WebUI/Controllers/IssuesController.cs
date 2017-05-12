using Microsoft.AspNet.Identity;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    [RoutePrefix("Group/{groupId:int:min(1)}")]
    public class IssuesController : BaseController
    {
        private readonly IIssueService _issueService = null;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }


        [HttpGet]
        public ActionResult Index(int groupId)
        {
            ViewBag.GroupId = groupId;
            var viewModels=_issueService.GetAll(groupId);

            return View("IssuesList", viewModels);
        }

        [HttpGet]
        public ActionResult Create(int groupId)
        {
            ViewBag.Action = "Create";
            var viewModel = new IssueViewModel()
            {
                GroupId = groupId,
                OpenedByUserId = User.Identity.GetUserId<int>()
            };

            return View("IssueForm", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int groupId, int id)
        {
            ViewBag.Action = "Edit";
            var viewModel = _issueService.Get(id);

            return View("IssueForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(int groupId, IssueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Create";
                ViewBag.GroupId = groupId;
                return View("IssueForm", viewModel);
            }

            viewModel.GroupId = groupId;
            _issueService.CreateOrUpdate(viewModel);
            return RedirectToAction("Index", new { groupId= groupId });
        }

        [HttpGet]
        public ActionResult Details(int groupId, int id)
        {
            var viewModel = _issueService.Get(groupId, id);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(CommentViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            _issueService.CreateOrUpdateComment(viewModel);
            return RedirectToAction("Details", new { groupId = viewModel.GroupId, id = viewModel.IssueId });
        }

        [HttpPost]
        public ActionResult RemoveComment(int CommentId)
        {
            _issueService.RemoveComment(CommentId);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}