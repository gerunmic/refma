namespace Refma.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LangElementGroups",
                c => new
                    {
                        LexemeId = c.Int(nullable: false),
                        LangElementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LexemeId, t.LangElementId })
                .ForeignKey("dbo.LangElements", t => t.LangElementId, cascadeDelete: true)
                .ForeignKey("dbo.LangElements", t => t.LexemeId)
                .Index(t => t.LexemeId)
                .Index(t => t.LangElementId);
            
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
                        ImageSmall = c.String(),
                        ImageBig = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        TargetLangId = c.Int(),
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
                .ForeignKey("dbo.Langs", t => t.TargetLangId)
                .Index(t => t.LangId)
                .Index(t => t.TargetLangId)
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
            
            CreateTable(
                "dbo.LangElementGroupTag",
                c => new
                    {
                        LexemeId = c.Int(nullable: false),
                        LangElementId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LexemeId, t.LangElementId, t.Id })
                .ForeignKey("dbo.LangElementGroups", t => new { t.LexemeId, t.LangElementId }, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Id, cascadeDelete: true)
                .Index(t => new { t.LexemeId, t.LangElementId })
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WebArticles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WebArticles", "LangId", "dbo.Langs");
            DropForeignKey("dbo.UserLangElements", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "TargetLangId", "dbo.Langs");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "LangId", "dbo.Langs");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserLangElements", "LangElementId", "dbo.LangElements");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LangElementTranslations", "LangElementId", "dbo.LangElements");
            DropForeignKey("dbo.LangElementTranslations", "LangId", "dbo.Langs");
            DropForeignKey("dbo.LangElementGroupTag", "Id", "dbo.Tags");
            DropForeignKey("dbo.LangElementGroupTag", new[] { "LexemeId", "LangElementId" }, "dbo.LangElementGroups");
            DropForeignKey("dbo.LangElementGroups", "LexemeId", "dbo.LangElements");
            DropForeignKey("dbo.LangElementGroups", "LangElementId", "dbo.LangElements");
            DropForeignKey("dbo.LangElements", "LangId", "dbo.Langs");
            DropIndex("dbo.LangElementGroupTag", new[] { "Id" });
            DropIndex("dbo.LangElementGroupTag", new[] { "LexemeId", "LangElementId" });
            DropIndex("dbo.WebArticles", new[] { "UserId" });
            DropIndex("dbo.WebArticles", new[] { "LangId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "TargetLangId" });
            DropIndex("dbo.AspNetUsers", new[] { "LangId" });
            DropIndex("dbo.UserLangElements", new[] { "LangElementId" });
            DropIndex("dbo.UserLangElements", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LangElementTranslations", new[] { "LangId" });
            DropIndex("dbo.LangElementTranslations", new[] { "LangElementId" });
            DropIndex("dbo.LangElements", "IX_UQ_ELEMENT");
            DropIndex("dbo.LangElementGroups", new[] { "LangElementId" });
            DropIndex("dbo.LangElementGroups", new[] { "LexemeId" });
            DropTable("dbo.LangElementGroupTag");
            DropTable("dbo.WebArticles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserLangElements");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.LangElementTranslations");
            DropTable("dbo.Tags");
            DropTable("dbo.Langs");
            DropTable("dbo.LangElements");
            DropTable("dbo.LangElementGroups");
        }
    }
}
