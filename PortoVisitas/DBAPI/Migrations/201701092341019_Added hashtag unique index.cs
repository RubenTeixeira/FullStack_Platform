namespace DBAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedhashtaguniqueindex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Hashtag", "Text", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Hashtag", new[] { "Text" });
        }
    }
}
