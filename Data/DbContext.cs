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

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Issue> Issues { get; set; }

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

            modelBuilder.Entity<Issue>()
                .HasRequired(i => i.Assignee)
                .WithMany(u => u.AssignedIssues)
                .HasForeignKey(i => i.AssignedToUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Issue>()
                .HasRequired(i => i.UserOpened)
                .WithMany(u => u.OpenedIssues)
                .HasForeignKey(i => i.OpenedByUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Issue>()
                .HasOptional(i => i.UserClosed)
                .WithMany(u => u.ClosedIssues)
                .HasForeignKey(i => i.ClosedByUserId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
