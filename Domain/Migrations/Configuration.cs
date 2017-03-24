namespace Domain.Migrations
{
    using Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Domain.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Domain.ApplicationDbContext";
        }

        protected override void Seed(Domain.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Tests.AddOrUpdate(
              
              new Test { Message = "Создать структуру проекта", Date=DateTime.Parse("2017-03-22") },
              new Test { Message = "Создать бизнес-объекты", Date = DateTime.Parse("2017-03-22") },
              new Test { Message = "Доделать остальное", Date = DateTime.Parse("2017-03-23") }
            );
            //
        }
    }
}
