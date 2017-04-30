using Data.Entities;
using Data.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.RoleTypes.AddRange(new List<RoleType> { 
                  new RoleType { Id = 1, Name = "Account" },
                  new RoleType { Id = 2, Name = "Group" } }
            );

            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, int, ApplicationUserRole>(context));

            roleManager.Create(new ApplicationRole() { Name = "Admin", RoleTypeId = 1 });
            roleManager.Create(new ApplicationRole() { Name = "User", RoleTypeId = 1 });
            roleManager.Create(new ApplicationRole() { Name = "Owner", RoleTypeId = 2 });
            roleManager.Create(new ApplicationRole() { Name = "Participant", RoleTypeId = 2 });

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim> (context));

            var admin = new ApplicationUser {
                UserName = "administrator",
                Email = "tasksmanagment.site@gmail.com",             
                EmailConfirmed = true,
                UserProfile = new UserProfile { FullName = "Administrator" }
            };

            userManager.Create(admin, "OfRqv3Z0");
            userManager.AddToRole(admin.Id, "Admin");


            base.Seed(context);
        }
    }
}
