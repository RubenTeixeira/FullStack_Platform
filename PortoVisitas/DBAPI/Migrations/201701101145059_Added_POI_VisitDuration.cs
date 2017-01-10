namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_POI_VisitDuration : DbMigration
    {
        public override void Up()
        {
            AddColumn("POI", "VisitDuration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("POI", "VisitDuration");
        }
    }
}
