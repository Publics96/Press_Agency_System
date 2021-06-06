namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMessageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message1 = c.String(),
                        Date = c.DateTime(nullable: false),
                        FromUser_Id = c.String(maxLength: 128),
                        ToUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUser_Id)
                .Index(t => t.FromUser_Id)
                .Index(t => t.ToUser_Id);
            
            CreateTable(
                "dbo.UserConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ConnectionId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserConnections", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ToUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "FromUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserConnections", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "ToUser_Id" });
            DropIndex("dbo.Messages", new[] { "FromUser_Id" });
            DropTable("dbo.UserConnections");
            DropTable("dbo.Messages");
        }
    }
}
