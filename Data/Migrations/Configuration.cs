using Data.Entities;

namespace Data.Migrations
{
    using Data.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Domain.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Tests.AddOrUpdate(
              
              new Test { Id = 1, Message = "Создать структуру проекта", Date=DateTime.Parse("2017-03-22") },
              new Test { Id = 2, Message = "Создать бизнес-объекты", Date = DateTime.Parse("2017-03-22") },
              new Test { Id = 3, Message = "Доделать остальное", Date = DateTime.Parse("2017-03-23") }
            );
            //
        }
    }
}
