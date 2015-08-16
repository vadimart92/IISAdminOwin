namespace IISAdmin.Owin.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Current : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobInfoes", "SignarRHubName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobInfoes", "SignarRHubName");
        }
    }
}
