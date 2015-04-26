namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lexemes2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" }, "dbo.LangElementGroups");
            DropIndex("dbo.Tags", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" });
            CreateTable(
                "dbo.TagLangElementGroups",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        LangElementGroup_LexemeId = c.Int(nullable: false),
                        LangElementGroup_LangElementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.LangElementGroup_LexemeId, t.LangElementGroup_LangElementId })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.LangElementGroups", t => new { t.LangElementGroup_LexemeId, t.LangElementGroup_LangElementId }, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => new { t.LangElementGroup_LexemeId, t.LangElementGroup_LangElementId });
            
            DropColumn("dbo.Tags", "LangElementGroup_LexemeId");
            DropColumn("dbo.Tags", "LangElementGroup_LangElementId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "LangElementGroup_LangElementId", c => c.Int());
            AddColumn("dbo.Tags", "LangElementGroup_LexemeId", c => c.Int());
            DropForeignKey("dbo.TagLangElementGroups", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" }, "dbo.LangElementGroups");
            DropForeignKey("dbo.TagLangElementGroups", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagLangElementGroups", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" });
            DropIndex("dbo.TagLangElementGroups", new[] { "Tag_Id" });
            DropTable("dbo.TagLangElementGroups");
            CreateIndex("dbo.Tags", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" });
            AddForeignKey("dbo.Tags", new[] { "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" }, "dbo.LangElementGroups", new[] { "LexemeId", "LangElementId" });
        }
    }
}
