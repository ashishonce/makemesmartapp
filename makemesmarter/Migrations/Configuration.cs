using makemesmarter.Models;

namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<makemesmarter.Models.makemesmarterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(makemesmarter.Models.makemesmarterContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Users.AddOrUpdate(
              p => p.UserId,
              new User { UserId="1",Name = "Andrew Peters",Token = "cOmtv712ZRM:APA91bHnt4JEtNPkAMtTy6d6xKVrlLHjcZl9vz8MpwDSipGcwjF5FZPtwfn20JLIGoWeC9tlK1BhNucKivwPNRuMaC_R4RBN3gHa0g-0NZz1sQ64SkYKvQMoqwvBVwr-y2Lzqtrrx8uR" },
              new User { UserId="2",Name = "Brice Lambson", Token = "cOmtv712ZRM:APA91bHnt4JEtNPkAMtTy6d6xKVrlLHjcZl9vz8MpwDSipGcwjF5FZPtwfn20JLIGoWeC9tlK1BhNucKivwPNRuMaC_R4RBN3gHa0g-0NZz1sQ64SkYKvQMoqwvBVwr-y2Lzqtrrx8uR" },
              new User { UserId="3",Name = "Rowan Miller" , Token = "q" }
            );

        }
    }
}
