namespace Refma.Migrations
{
    using Refma.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Refma.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Refma.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (context.Langs.Count() == 0)
            {

                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
                List<Lang> entries = new List<Lang>();
                foreach (CultureInfo culture in cultures)
                {
                    entries.Add(new Lang() { ID = culture.LCID, Code = culture.Name, Name = culture.NativeName, ImageSmall = String.Format("{0}.png", culture.Name), ImageBig = String.Format("{0}.png", culture.Name) });
                }
                context.Langs.AddOrUpdate(entries.ToArray<Lang>());
            }
        }
    }
}
