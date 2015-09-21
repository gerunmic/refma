namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SentenceModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sentences",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LangId = c.Int(nullable: false),
                        Pattern = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Langs", t => t.LangId, cascadeDelete: true)
                .Index(t => t.LangId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sentences", "LangId", "dbo.Langs");
            DropIndex("dbo.Sentences", new[] { "LangId" });
            DropTable("dbo.Sentences");
        }
    }
}
