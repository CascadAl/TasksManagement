using Data.Entities;

namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Tests.AddOrUpdate(
              t => t.Message,
              new Test { Message = "Create project structure", Date = DateTime.Now },
              new Test { Message = "Implemented basic exception handler", Date = DateTime.Now },
              new Test { Message = "Implement groups creation", Date = DateTime.Now }
            );

            context.RoleTypes.AddOrUpdate(
                r=>r.Name,
                new RoleType(){Id = 1, Name = "Account"},
                new RoleType(){Id=2, Name="Group"}
            );
            
            context.Roles.AddOrUpdate(
                r=>r.Name,
                new ApplicationRole() {Id = 1, Name = "Admin", RoleTypeId = 1},
                new ApplicationRole() {Id = 2, Name = "Owner", RoleTypeId = 2},
                new ApplicationRole() { Id = 3, Name = "Participant", RoleTypeId = 2 }
                );
        }
    }
}
