using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public DbSet<Test> Tests { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<RoleType> RoleTypes { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here
            modelBuilder.Entity<GroupMember>()
                .HasRequired(t => t.User)
                .WithMany(t => t.GroupMembers)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<GroupMember>()
                 .HasRequired(t => t.Group)
                 .WithMany(t => t.Members)
                 .HasForeignKey(t => t.GroupId);

            base.OnModelCreating(modelBuilder);
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
