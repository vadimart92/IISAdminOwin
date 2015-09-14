namespace IISAdmin.Owin.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Current1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.JobInfoes");
            AlterColumn("dbo.JobInfoes", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.JobInfoes", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.JobInfoes");
            AlterColumn("dbo.JobInfoes", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.JobInfoes", "Id");
        }
    }
}
