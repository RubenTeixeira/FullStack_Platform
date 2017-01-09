namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class percurso : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("POI", "Approved_Id", "AspNetUsers");
            DropForeignKey("POI", "Creator_Id", "AspNetUsers");
            DropIndex("POI", new[] { "Approved_Id" });
            DropIndex("POI", new[] { "Creator_Id" });
            CreateTable(
                "Percurso",
                c => new
                    {
                        PercursoID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250, storeType: "nvarchar"),
                        Description = c.String(maxLength: 250, storeType: "nvarchar"),
                        Creator = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.PercursoID);
            
            CreateTable(
                "Percurso_POI",
                c => new
                    {
                        PercursoID = c.Int(nullable: false),
                        POIID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PercursoID, t.POIID })
                .ForeignKey("Percurso", t => t.PercursoID, cascadeDelete: true)
                .ForeignKey("POI", t => t.POIID, cascadeDelete: true)
                .Index(t => t.PercursoID)
                .Index(t => t.POIID);
            
            AddColumn("POI", "OpenHour", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("POI", "CloseHour", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("POI", "Creator", c => c.String(unicode: false));
            AddColumn("POI", "Approved", c => c.String(unicode: false));
            DropColumn("POI", "Approved_Id");
            DropColumn("POI", "Creator_Id");
        }
        
        public override void Down()
        {
                     
            AddColumn("POI", "Creator_Id", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("POI", "Approved_Id", c => c.String(maxLength: 128, storeType: "nvarchar"));
            DropForeignKey("Percurso_POI", "POIID", "POI");
            DropForeignKey("Percurso_POI", "PercursoID", "Percurso");
            DropIndex("Percurso_POI", new[] { "POIID" });
            DropIndex("Percurso_POI", new[] { "PercursoID" });
            DropColumn("POI", "Approved");
            DropColumn("POI", "Creator");
            DropColumn("POI", "CloseHour");
            DropColumn("POI", "OpenHour");
            DropTable("Percurso_POI");
            DropTable("Percurso");
            CreateIndex("POI", "Creator_Id");
            CreateIndex("POI", "Approved_Id");
            AddForeignKey("POI", "Creator_Id", "AspNetUsers", "Id");
            AddForeignKey("POI", "Approved_Id", "AspNetUsers", "Id");
        }
    }
}
