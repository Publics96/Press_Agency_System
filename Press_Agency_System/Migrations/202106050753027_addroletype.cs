namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addroletype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "roleType_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "roleType_Id");
            AddForeignKey("dbo.AspNetUsers", "roleType_Id", "dbo.AspNetRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "roleType_Id", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUsers", new[] { "roleType_Id" });
            DropColumn("dbo.AspNetUsers", "roleType_Id");
        }
    }
}
