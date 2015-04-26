namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lexemes3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TagLangElementGroups", newName: "LangElementGroupTag");
            RenameColumn(table: "dbo.LangElementGroupTag", name: "Tag_Id", newName: "Id");
            RenameColumn(table: "dbo.LangElementGroupTag", name: "LangElementGroup_LexemeId", newName: "LexemeId");
            RenameColumn(table: "dbo.LangElementGroupTag", name: "LangElementGroup_LangElementId", newName: "LangElementId");
            RenameIndex(table: "dbo.LangElementGroupTag", name: "IX_LangElementGroup_LexemeId_LangElementGroup_LangElementId", newName: "IX_LexemeId_LangElementId");
            RenameIndex(table: "dbo.LangElementGroupTag", name: "IX_Tag_Id", newName: "IX_Id");
            DropPrimaryKey("dbo.LangElementGroupTag");
            AddPrimaryKey("dbo.LangElementGroupTag", new[] { "LexemeId", "LangElementId", "Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.LangElementGroupTag");
            AddPrimaryKey("dbo.LangElementGroupTag", new[] { "Tag_Id", "LangElementGroup_LexemeId", "LangElementGroup_LangElementId" });
            RenameIndex(table: "dbo.LangElementGroupTag", name: "IX_Id", newName: "IX_Tag_Id");
            RenameIndex(table: "dbo.LangElementGroupTag", name: "IX_LexemeId_LangElementId", newName: "IX_LangElementGroup_LexemeId_LangElementGroup_LangElementId");
            RenameColumn(table: "dbo.LangElementGroupTag", name: "LangElementId", newName: "LangElementGroup_LangElementId");
            RenameColumn(table: "dbo.LangElementGroupTag", name: "LexemeId", newName: "LangElementGroup_LexemeId");
            RenameColumn(table: "dbo.LangElementGroupTag", name: "Id", newName: "Tag_Id");
            RenameTable(name: "dbo.LangElementGroupTag", newName: "TagLangElementGroups");
        }
    }
}
