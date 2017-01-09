namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltitudePOI : DbMigration
    {
        public override void Up()
        {
            AddColumn("POI", "Altitude", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("POI", "Altitude");
        }
    }
}
