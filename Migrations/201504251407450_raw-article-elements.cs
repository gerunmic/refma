namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rawarticleelements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RawArticleElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WebArticleId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RawArticleElements");
        }
    }
}
