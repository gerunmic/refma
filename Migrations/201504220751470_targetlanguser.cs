namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class targetlanguser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TargetLangId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "TargetLangId");
            AddForeignKey("dbo.AspNetUsers", "TargetLangId", "dbo.Langs", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "TargetLangId", "dbo.Langs");
            DropIndex("dbo.AspNetUsers", new[] { "TargetLangId" });
            DropColumn("dbo.AspNetUsers", "TargetLangId");
        }
    }
}
