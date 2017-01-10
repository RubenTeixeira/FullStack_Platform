namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Percurso_HourAndOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("Percurso", "StartHour", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("Percurso", "PercursoPOIsOrder", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Percurso", "PercursoPOIsOrder");
            DropColumn("Percurso", "StartHour");
        }
    }
}
