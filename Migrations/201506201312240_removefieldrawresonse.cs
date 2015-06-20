namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removefieldrawresonse : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LangElementTranslations", "RawResponse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LangElementTranslations", "RawResponse", c => c.String());
        }
    }
}
