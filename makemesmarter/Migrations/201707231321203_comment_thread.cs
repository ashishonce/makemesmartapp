namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment_thread : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentThreads",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Status = c.String(),
                        FileType = c.String(),
                        UserLikedCount = c.Int(nullable: false),
                        SentimentValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommentThreads");
        }
    }
}
