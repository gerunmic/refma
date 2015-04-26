namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lexemes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LangElementGroups",
                c => new
                    {
                        LexemeId = c.Int(nullable: false),
                        LangElementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LexemeId, t.LangElementId })
                .ForeignKey("dbo.LangElements", t => t.LangElementId, cascadeDelete: true)
                .ForeignKey("dbo.LangElements", t => t.LexemeId)
                .Index(t => t.LexemeId)
                .Index(t => t.LangElementId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        LangElementGroup_LexemeId = c.Int(),
                        LangElementGroup_LangElementId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LangElementGroups", t => new { t.LangElementGroup_LexemeId, t.LangElementGroup_LangElementId })
                .Index(t => new { t.LangElementGroup_LexemeId, t.LangElementGroup_LangElementId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" }, "dbo.LangElementGroups");
            DropForeignKey("dbo.LangElementGroups", "LexemeId", "dbo.LangElements");
            DropForeignKey("dbo.LangElementGroups", "LangElementId", "dbo.LangElements");
            DropIndex("dbo.Tags", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" });
            DropIndex("dbo.LangElementGroups", new[] { "LangElementId" });
            DropIndex("dbo.LangElementGroups", new[] { "LexemeId" });
            DropTable("dbo.Tags");
            DropTable("dbo.LangElementGroups");
        }
    }
}
