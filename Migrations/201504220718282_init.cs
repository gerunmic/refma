namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LangElements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LangId = c.Int(nullable: false),
                        Value = c.String(nullable: false, maxLength: 450),
                        Occurrency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Langs", t => t.LangId)
                .Index(t => new { t.LangId, t.Value }, unique: true, name: "IX_UQ_ELEMENT");
            
            CreateTable(
                "dbo.Langs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LangElementTranslations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LangElementId = c.Int(nullable: false),
                        LangId = c.Int(nullable: false),
                        Translation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Langs", t => t.LangId)
                .ForeignKey("dbo.LangElements", t => t.LangElementId, cascadeDelete: true)
                .Index(t => t.LangElementId)
                .Index(t => t.LangId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserLangElements",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LangElementId = c.Int(nullable: false),
                        Knowledge = c.Int(nullable: false),
                        Occurency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.LangElementId })
                .ForeignKey("dbo.LangElements", t => t.LangElementId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.LangElementId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LangId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Langs", t => t.LangId)
                .Index(t => t.LangId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WebArticles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LangId = c.Int(nullable: false),
                        Title = c.String(),
                        URL = c.String(),
                        SourceText = c.String(),
                        PlainText = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Langs", t => t.LangId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.LangId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WebArticles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WebArticles", "LangId", "dbo.Langs");
            DropForeignKey("dbo.UserLangElements", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "LangId", "dbo.Langs");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserLangElements", "LangElementId", "dbo.LangElements");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LangElementTranslations", "LangElementId", "dbo.LangElements");
            DropForeignKey("dbo.LangElementTranslations", "LangId", "dbo.Langs");
            DropForeignKey("dbo.LangElements", "LangId", "dbo.Langs");
            DropIndex("dbo.WebArticles", new[] { "UserId" });
            DropIndex("dbo.WebArticles", new[] { "LangId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "LangId" });
            DropIndex("dbo.UserLangElements", new[] { "LangElementId" });
            DropIndex("dbo.UserLangElements", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LangElementTranslations", new[] { "LangId" });
            DropIndex("dbo.LangElementTranslations", new[] { "LangElementId" });
            DropIndex("dbo.LangElements", "IX_UQ_ELEMENT");
            DropTable("dbo.WebArticles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserLangElements");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.LangElementTranslations");
            DropTable("dbo.Langs");
            DropTable("dbo.LangElements");
        }
    }
}
