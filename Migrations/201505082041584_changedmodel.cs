namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedmodel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LangElements", "IX_UQ_ELEMENT");
            CreateTable(
                "dbo.WebArticleElements",
                c => new
                    {
                        WebArticleId = c.Int(nullable: false),
                        LangElementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WebArticleId, t.LangElementId })
                .ForeignKey("dbo.LangElements", t => t.LangElementId, cascadeDelete: true)
                .ForeignKey("dbo.WebArticles", t => t.WebArticleId, cascadeDelete: true)
                .Index(t => t.WebArticleId)
                .Index(t => t.LangElementId);
            
            CreateIndex("dbo.LangElements", "LangId");
            DropColumn("dbo.LangElements", "Occurrency");
            DropColumn("dbo.UserLangElements", "Occurency");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserLangElements", "Occurency", c => c.Int(nullable: false));
            AddColumn("dbo.LangElements", "Occurrency", c => c.Int(nullable: false));
            DropForeignKey("dbo.WebArticleElements", "WebArticleId", "dbo.WebArticles");
            DropForeignKey("dbo.WebArticleElements", "LangElementId", "dbo.LangElements");
            DropIndex("dbo.WebArticleElements", new[] { "LangElementId" });
            DropIndex("dbo.WebArticleElements", new[] { "WebArticleId" });
            DropIndex("dbo.LangElements", new[] { "LangId" });
            DropTable("dbo.WebArticleElements");
            CreateIndex("dbo.LangElements", new[] { "LangId", "Value" }, unique: true, name: "IX_UQ_ELEMENT");
        }
    }
}
