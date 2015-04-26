namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerawarticles : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RawArticleElements");
        }
        
        public override void Down()
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
    }
}
