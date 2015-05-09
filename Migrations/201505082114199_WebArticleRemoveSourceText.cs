namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebArticleRemoveSourceText : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.WebArticles", "SourceText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WebArticles", "SourceText", c => c.String());
        }
    }
}
