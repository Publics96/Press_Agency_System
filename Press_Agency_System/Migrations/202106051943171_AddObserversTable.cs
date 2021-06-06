namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddObserversTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Observers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Observers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Observers", new[] { "UserId" });
            DropTable("dbo.Observers");
        }
    }
}
