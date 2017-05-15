using Data.Entities;
using Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Classes;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using WebUI.Controllers;
using System.Web.Mvc;
using System.Web;
using System.Security.Principal;
using System.Security.Claims;
using System.Web.Routing;
using Services.Models;

namespace UnitTests
{
    [TestClass]
    public class GroupServiceTests
    {

        //[TestInitialize]
        //public void TestInit()
        //{
        //    var mockGroupMember = new Mock<IGroupMemberRepository>();
        //    var mockAppRole = new Mock<IApplicationRoleRepository>();
        //    var mockUser = new Mock<IUserRepository>();
        //    var mockIssue = new Mock<IIssueRepository>();
        //    var mockGroup = new Mock<IGroupRepository>();

        //    groupService = new GroupService(mockGroup.Object, mockUser.Object, mockAppRole.Object, mockGroupMember.Object, mockIssue.Object);
        //}


        [TestMethod]
        public void IsGroupParticipant_CallingHas()
        {
            // Arrange
            var mockGroupMember = new Mock<IGroupMemberRepository>();
            IGroupService groupService = new GroupService(null, null, null, mockGroupMember.Object, null);

            groupService.IsGroupParticipant(1,1);

            mockGroupMember.Verify(gm => gm.Has(It.IsAny<Expression<Func<GroupMember, bool>>>()));
        }

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void IsGroupOwner_NoOwnerRole()
        //{
        //    // Arrange
        //    var mock = new Mock<IGroupService>();
        //    mock.Setup(m => m.IsGroupOwner(1, 1)).Returns(false);

        //    var context = new Mock<HttpContextBase>();
        //    var identity = new GenericIdentity("dominik.ernst@xyz123.de");
        //    identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "1"));
        //    var principal = new GenericPrincipal(identity, new[] { "user" });
        //    context.Setup(s => s.User).Returns(principal);

        //    var groupController = new GroupController(mock.Object, null);
        //    groupController.ControllerContext = new ControllerContext(context.Object, new RouteData(), groupController);

        //    // Act
        //    ViewResult result = groupController.Remove(1) as ViewResult;
        //}


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMember_RejectsWhenNotOwner()
        {
            // Arrange
            var mockGroupMember = new Mock<IGroupMemberRepository>();
            IGroupService groupService = new GroupService(null, null, null, mockGroupMember.Object, null);
            
            mockGroupMember.Setup(m => m.GetRole(0, 0)).Returns(RoleNames.ROLE_PARTICIPANT);
            var viewModel = new GroupMemberViewModel();

            // Act
            groupService.AddMember(viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMember_RejectsWhenAlreadyinGroup()
        {
            // Arrange
            var mockGroupMember = new Mock<IGroupMemberRepository>();
            IGroupService groupService = new GroupService(null, null, null, mockGroupMember.Object, null);

            mockGroupMember.Setup(m => m.GetRole(0, 0)).Returns(RoleNames.ROLE_OWNER);
            mockGroupMember.Setup(m => m.IsInGroup(0, 0)).Returns(true);
            var viewModel = new GroupMemberViewModel();

            // Act
            groupService.AddMember(viewModel);
        }
    }
}
