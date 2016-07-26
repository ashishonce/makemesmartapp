namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QueryModels",
                c => new
                {
                    QueryId = c.Int(nullable: false, identity: true),
                    Query = c.String(),
                    Reply = c.String(),
                })
                .PrimaryKey(t => t.QueryId);
        }
            
       
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.QueryModels");
        }
    }
}
