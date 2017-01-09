namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHashtags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hashtag",
                c => new
                    {
                        HashtagID = c.Int(nullable: false, identity: true),
                        Text = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.HashtagID);
            
            CreateTable(
                "dbo.POIHashtag",
                c => new
                    {
                        Hashtag_HashtagID = c.Int(nullable: false),
                        POI_POIID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hashtag_HashtagID, t.POI_POIID })
                .ForeignKey("dbo.Hashtag", t => t.Hashtag_HashtagID, cascadeDelete: true)
                .ForeignKey("dbo.POI", t => t.POI_POIID, cascadeDelete: true)
                .Index(t => t.Hashtag_HashtagID)
                .Index(t => t.POI_POIID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.POIHashtag", "POI_POIID", "dbo.POI");
            DropForeignKey("dbo.POIHashtag", "Hashtag_HashtagID", "dbo.Hashtag");
            DropIndex("dbo.POIHashtag", new[] { "POI_POIID" });
            DropIndex("dbo.POIHashtag", new[] { "Hashtag_HashtagID" });
            DropTable("dbo.POIHashtag");
            DropTable("dbo.Hashtag");
        }
    }
}
