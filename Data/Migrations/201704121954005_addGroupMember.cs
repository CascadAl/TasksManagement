namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGroupMember : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserGroups", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "Group_Id", "dbo.Groups");
            DropIndex("dbo.ApplicationUserGroups", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "Group_Id" });
            CreateTable(
                "dbo.GroupMembers",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.UserId })
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            AddColumn("dbo.AspNetRoles", "RoleTypeId", c => c.Int(nullable: false));
            DropTable("dbo.ApplicationUserGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        ApplicationUser_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Group_Id });
            
            DropForeignKey("dbo.GroupMembers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupMembers", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GroupMembers", "GroupId", "dbo.Groups");
            DropIndex("dbo.GroupMembers", new[] { "RoleId" });
            DropIndex("dbo.GroupMembers", new[] { "UserId" });
            DropIndex("dbo.GroupMembers", new[] { "GroupId" });
            DropColumn("dbo.AspNetRoles", "RoleTypeId");
            DropTable("dbo.GroupMembers");
            CreateIndex("dbo.ApplicationUserGroups", "Group_Id");
            CreateIndex("dbo.ApplicationUserGroups", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserGroups", "Group_Id", "dbo.Groups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserGroups", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
