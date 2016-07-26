namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 200),
                        Name = c.String(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            CreateTable(
                "dbo.QueryModel",
                c => new
                {
                    Query = c.String(nullable: false, maxLength: 200),
                    Reply = c.String(),
                })
                .PrimaryKey(t => t.Query);

        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
