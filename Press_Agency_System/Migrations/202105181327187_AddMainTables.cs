namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMainTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InteractedPosts",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        PostId = c.Int(nullable: false),
                        Like = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PostId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostTitle = c.String(nullable: false, maxLength: 50),
                        PostBody = c.String(nullable: false, maxLength: 3000),
                        PostDate = c.DateTime(nullable: false),
                        PostType = c.String(nullable: false),
                        ImagePath = c.String(),
                        State = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageContent = c.String(nullable: false, maxLength: 1000),
                        Datetime = c.DateTime(nullable: false),
                        EditorUser_Id = c.String(maxLength: 128),
                        ViwerUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.EditorUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ViwerUser_Id)
                .Index(t => t.EditorUser_Id)
                .Index(t => t.ViwerUser_Id);
            
            CreateTable(
                "dbo.SavedPosts",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PostId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Email", c => c.String());
            AddColumn("dbo.AspNetUsers", "PhotoPath", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SavedPosts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SavedPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Questions", "ViwerUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Questions", "EditorUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InteractedPosts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InteractedPosts", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SavedPosts", new[] { "UserId" });
            DropIndex("dbo.SavedPosts", new[] { "PostId" });
            DropIndex("dbo.Questions", new[] { "ViwerUser_Id" });
            DropIndex("dbo.Questions", new[] { "EditorUser_Id" });
            DropIndex("dbo.InteractedPosts", new[] { "UserId" });
            DropIndex("dbo.InteractedPosts", new[] { "PostId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "PhotoPath");
            DropColumn("dbo.AspNetUsers", "Email");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.SavedPosts");
            DropTable("dbo.Questions");
            DropTable("dbo.Posts");
            DropTable("dbo.InteractedPosts");
        }
    }
}
