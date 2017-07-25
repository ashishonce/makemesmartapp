namespace makemesmarter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_updateCommentCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReviewerDatas",
                c => new
                    {
                        ReviewerID = c.Int(nullable: false, identity: true),
                        Alias = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.ReviewerID);
            
            CreateTable(
                "dbo.ReviewScores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReviewerID = c.Int(nullable: false),
                        FileType = c.String(),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ReviewerDatas", t => t.ReviewerID, cascadeDelete: true)
                .Index(t => t.ReviewerID);
            
            AddColumn("dbo.CommentThreads", "commentCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewScores", "ReviewerID", "dbo.ReviewerDatas");
            DropIndex("dbo.ReviewScores", new[] { "ReviewerID" });
            DropColumn("dbo.CommentThreads", "commentCategory");
            DropTable("dbo.ReviewScores");
            DropTable("dbo.ReviewerDatas");
        }
    }
}
