namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public sealed partial class modifyingTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
            AlterColumn("dbo.Posts", "State", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Posts", "State", c => c.Byte(nullable: false));
            DropColumn("dbo.AspNetUsers", "Phone");
        }
    }
}
