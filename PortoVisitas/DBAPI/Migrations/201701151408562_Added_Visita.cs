namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Visita : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visita",
                c => new
                    {
                        VisitaID = c.Int(nullable: false, identity: true),
                        Creator = c.String(unicode: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Percurso_PercursoID = c.Int(),
                    })
                .PrimaryKey(t => t.VisitaID)
                .ForeignKey("dbo.Percurso", t => t.Percurso_PercursoID)
                .Index(t => t.Percurso_PercursoID);
            
            AlterColumn("dbo.Percurso", "PercursoPOIsOrder", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visita", "Percurso_PercursoID", "dbo.Percurso");
            DropIndex("dbo.Visita", new[] { "Percurso_PercursoID" });
            AlterColumn("dbo.Percurso", "PercursoPOIsOrder", c => c.String(unicode: false));
            DropTable("dbo.Visita");
        }
    }
}
