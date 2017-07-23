namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment_thread_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentThreads", "PrAuthorId", c => c.String());
            AddColumn("dbo.CommentThreads", "PullRequestId", c => c.Int(nullable: false));
            AddColumn("dbo.CommentThreads", "IsUseful", c => c.Int(nullable: false));
            AddColumn("dbo.CommentThreads", "JoinedComments", c => c.String());
            AddColumn("dbo.CommentThreads", "CumlativeLikes", c => c.Int(nullable: false));
            AddColumn("dbo.CommentThreads", "FilePath", c => c.String());
            DropColumn("dbo.CommentThreads", "Comment");
            DropColumn("dbo.CommentThreads", "UserLikedCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommentThreads", "UserLikedCount", c => c.Int(nullable: false));
            AddColumn("dbo.CommentThreads", "Comment", c => c.String());
            DropColumn("dbo.CommentThreads", "FilePath");
            DropColumn("dbo.CommentThreads", "CumlativeLikes");
            DropColumn("dbo.CommentThreads", "JoinedComments");
            DropColumn("dbo.CommentThreads", "IsUseful");
            DropColumn("dbo.CommentThreads", "PullRequestId");
            DropColumn("dbo.CommentThreads", "PrAuthorId");
        }
    }
}
