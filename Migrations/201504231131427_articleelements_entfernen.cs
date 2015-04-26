namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articleelements_entfernen : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WebArticleLangElements", "WebArticleId", "dbo.WebArticles");
            DropForeignKey("dbo.WebArticleLangElements", "LangElementId", "dbo.LangElements");
            DropIndex("dbo.WebArticleLangElements", new[] { "WebArticleId" });
            DropIndex("dbo.WebArticleLangElements", new[] { "LangElementId" });
            DropTable("dbo.WebArticleLangElements");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WebArticleLangElements",
                c => new
                    {
                        WebArticleId = c.Int(nullable: false),
                        LangElementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WebArticleId, t.LangElementId });
            
            CreateIndex("dbo.WebArticleLangElements", "LangElementId");
            CreateIndex("dbo.WebArticleLangElements", "WebArticleId");
            AddForeignKey("dbo.WebArticleLangElements", "LangElementId", "dbo.LangElements", "ID", cascadeDelete: true);
            AddForeignKey("dbo.WebArticleLangElements", "WebArticleId", "dbo.WebArticles", "ID", cascadeDelete: true);
        }
    }
}
