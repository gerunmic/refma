namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articleelements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebArticleLangElements",
                c => new
                    {
                        WebArticle_ID = c.Int(nullable: false),
                        LangElement_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WebArticle_ID, t.LangElement_ID })
                .ForeignKey("dbo.WebArticles", t => t.WebArticle_ID, cascadeDelete: true)
                .ForeignKey("dbo.LangElements", t => t.LangElement_ID, cascadeDelete: true)
                .Index(t => t.WebArticle_ID)
                .Index(t => t.LangElement_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WebArticleLangElements", "LangElement_ID", "dbo.LangElements");
            DropForeignKey("dbo.WebArticleLangElements", "WebArticle_ID", "dbo.WebArticles");
            DropIndex("dbo.WebArticleLangElements", new[] { "LangElement_ID" });
            DropIndex("dbo.WebArticleLangElements", new[] { "WebArticle_ID" });
            DropTable("dbo.WebArticleLangElements");
        }
    }
}
