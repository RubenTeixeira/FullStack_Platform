namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Percurso_FinishHour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Percurso", "FinishHour", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Percurso", "FinishHour");
        }
    }
}
