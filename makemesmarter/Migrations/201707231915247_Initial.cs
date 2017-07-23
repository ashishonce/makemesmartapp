namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentThreads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        JoinedComments = c.String(),
                        Status = c.String(),
                        ThreadCount = c.Int(nullable: false),
                        IsUseful = c.Int(nullable: false),
                        CumlativeLikes = c.Int(nullable: false),
                        FileType = c.String(),
                        FilePath = c.String(),
                        SentimentValue = c.Double(nullable: false),
                        PrAuthorId = c.String(),
                        PullRequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.CommentThreads");
        }
    }
}
