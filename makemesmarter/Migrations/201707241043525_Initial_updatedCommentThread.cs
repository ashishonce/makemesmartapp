namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_updatedCommentThread : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentThreads", "commentInitiator", c => c.String());
            AddColumn("dbo.CommentThreads", "initiatorCommentLength", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommentThreads", "initiatorCommentLength");
            DropColumn("dbo.CommentThreads", "commentInitiator");
        }
    }
}
