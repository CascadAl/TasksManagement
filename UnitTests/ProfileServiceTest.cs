using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Data.Repository;
using Data.Entities;
using Services.Classes;
using Services.DataTransferObjects;

namespace UnitTests
{
    [TestClass]
    public class ProfileServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetProfile_UserNotExists()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();
            ApplicationUser user = null;
            mock.Setup(a => a.GetUserById(0)).Returns(user);
            ProfileService service = new ProfileService(mock.Object);

            //Act
            ProfileDTO profile = service.GetProfile(0);
        }

        [TestMethod]
        public void GetProfile_NotNullAvatar()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();
            ApplicationUser user = new ApplicationUser { UserProfile = new UserProfile { AvatarName = "MyAvatar" } };
            mock.Setup(a => a.GetUserById(1)).Returns(user);
            ProfileService service = new ProfileService(mock.Object);
            service.AvatarFolder = "C:\\SomeFolder";
            string expected = "C:\\SomeFolder\\MyAvatar";

            //Act
            ProfileDTO profile = service.GetProfile(1);

            //Accert
            Equals(expected, profile.AvatarPath);
        }

        [TestMethod]
        public void GetProfile_NullAvatar()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();
            ApplicationUser user = new ApplicationUser { UserProfile = new UserProfile { AvatarName = null } };
            mock.Setup(a => a.GetUserById(1)).Returns(user);
            ProfileService service = new ProfileService(mock.Object);
            service.DefaultAvatar = "C:\\SomeFolder\\DefaultAvatar.png";

            //Act
            ProfileDTO profile = service.GetProfile(1);

            //Accert
            Equals(service.DefaultAvatar, profile.AvatarPath);
        }
    }


}
