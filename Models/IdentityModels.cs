using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Refma.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public int LangId { get; set; }
        [ForeignKey("LangId")]
        public virtual Lang Lang { get; set; }

        public int? TargetLangId { get; set; }
        [ForeignKey("TargetLangId")]
        public virtual Lang TargetLang { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<LangElement> LangElements { get; set; }
        public DbSet<LangElementTranslation> LangElementTranslations { get; set; }
        public DbSet<UserLangElement> UserLangElements { get; set; }

        public DbSet<WebArticle> WebArticles { get; set; }
        public DbSet<WebArticleElement> WebArticleElements { get; set; }
        public DbSet<Lang> Langs { get; set; }
        public DbSet<LangElementGroup> LangElementGroups { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<RawTranslationResponse> RawTranslationReponses { get; set; }

        public DbSet<Sentence> Sentences { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(c => c.Lang)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LangElement>()
                .HasRequired(c => c.Lang)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WebArticle>()
                .HasRequired(c => c.Lang)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LangElementTranslation>()
                .HasRequired(c => c.Lang)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RawTranslationResponse>()
                .HasRequired(c => c.Lang)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LangElementGroup>()
                .HasRequired(c => c.Lexeme)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LangElementGroup>()
                .HasMany<Tag>(t => t.Tags)
                .WithMany(l => l.LangElementGroups)
                .Map(tl =>
                {
                    tl.MapLeftKey(new string[] { "LexemeId", "LangElementId" });
                    tl.MapRightKey("Id");
                    tl.ToTable("LangElementGroupTag");

                });
        }


    }
}
