namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateColumnsforPostTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "PostTitle", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Posts", "PostBody", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "PostBody", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("dbo.Posts", "PostTitle", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
