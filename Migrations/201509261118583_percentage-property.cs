namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class percentageproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WebArticles", "PercentageKnown", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebArticles", "PercentageKnown");
        }
    }
}
