namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testmirationforchat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserConnections", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserConnections", new[] { "UserId" });
            AddColumn("dbo.UserConnections", "UserId_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserConnections", "UserId_Id");
            AddForeignKey("dbo.UserConnections", "UserId_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.UserConnections", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserConnections", "UserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.UserConnections", "UserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserConnections", new[] { "UserId_Id" });
            DropColumn("dbo.UserConnections", "UserId_Id");
            CreateIndex("dbo.UserConnections", "UserId");
            AddForeignKey("dbo.UserConnections", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
