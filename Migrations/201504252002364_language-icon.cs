namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class languageicon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Langs", "ImageSmall", c => c.String());
            AddColumn("dbo.Langs", "ImageBig", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Langs", "ImageBig");
            DropColumn("dbo.Langs", "ImageSmall");
        }
    }
}
