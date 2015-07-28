namespace IISAdmin.Owin.DAL.EF.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobInfoes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        JobId = c.Int(nullable: false),
                        SerializedInfo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("HangFire.Job", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobInfoes", "JobId", "HangFire.Job");
            DropIndex("dbo.JobInfoes", new[] { "JobId" });
            DropTable("dbo.JobInfoes");
        }
    }
}
