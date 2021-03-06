﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Data.Entities
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<int>, IEntity
    {
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
        public virtual ICollection<Issue> AssignedIssues { get; set; }
        public virtual ICollection<Issue> OpenedIssues { get; set; }
        public virtual ICollection<Issue> ClosedIssues { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser()
        {
            GroupMembers = new List<GroupMember>();
            AssignedIssues = new List<Issue>();
            OpenedIssues = new List<Issue>();
            ClosedIssues = new List<Issue>();
        }
    }
}
