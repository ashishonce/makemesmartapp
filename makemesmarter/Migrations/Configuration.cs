namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using makemesmarter.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<makemesmarter.Models.makemesmarterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(makemesmarter.Models.makemesmarterContext context)
        {
            context.Users.AddOrUpdate(p => p.UserId, 
                new User
                {
                    UserId = "123",
                    Name = "Mazhar"
                },
                new User
                {
                    UserId = "124",
                    Name = "Anant"
                }); 
        }
    }
}
