namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranslationResponseField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LangElementTranslations", "RawResponse", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LangElementTranslations", "RawResponse");
        }
    }
}
