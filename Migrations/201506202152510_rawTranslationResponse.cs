namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rawTranslationResponse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RawTranslationResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LangElementId = c.Int(nullable: false),
                        LangId = c.Int(nullable: false),
                        Provider = c.String(),
                        Response = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Langs", t => t.LangId)
                .ForeignKey("dbo.LangElements", t => t.LangElementId, cascadeDelete: true)
                .Index(t => t.LangElementId)
                .Index(t => t.LangId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RawTranslationResponses", "LangElementId", "dbo.LangElements");
            DropForeignKey("dbo.RawTranslationResponses", "LangId", "dbo.Langs");
            DropIndex("dbo.RawTranslationResponses", new[] { "LangId" });
            DropIndex("dbo.RawTranslationResponses", new[] { "LangElementId" });
            DropTable("dbo.RawTranslationResponses");
        }
    }
}
