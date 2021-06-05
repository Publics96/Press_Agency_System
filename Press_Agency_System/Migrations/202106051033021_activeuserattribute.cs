namespace Press_Agency_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeuserattribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "activeUser", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "activeUser");
        }
    }
}
