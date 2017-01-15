namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Visita1 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Visita", "Percurso_PercursoID", "dbo.Percurso");
            //DropIndex("dbo.Visita", new[] { "Percurso_PercursoID" });
            //RenameColumn(table: "dbo.Visita", name: "Percurso_PercursoID", newName: "PercursoID");
            AlterColumn("dbo.Visita", "PercursoID", c => c.Int(nullable: false));
            CreateIndex("dbo.Visita", "PercursoID");
            AddForeignKey("dbo.Visita", "PercursoID", "dbo.Percurso", "PercursoID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visita", "PercursoID", "dbo.Percurso");
            DropIndex("dbo.Visita", new[] { "PercursoID" });
            AlterColumn("dbo.Visita", "PercursoID", c => c.Int());
            RenameColumn(table: "dbo.Visita", name: "PercursoID", newName: "Percurso_PercursoID");
            CreateIndex("dbo.Visita", "Percurso_PercursoID");
            AddForeignKey("dbo.Visita", "Percurso_PercursoID", "dbo.Percurso", "PercursoID");
        }
    }
}
