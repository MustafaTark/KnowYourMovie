namespace IMDB2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Actors", "Image");
            DropColumn("dbo.Actors", "IsFavorite");
            DropColumn("dbo.Movies", "Image");
            DropColumn("dbo.Movies", "IsFavorite");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "IsFavorite", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movies", "Image", c => c.Binary());
            AddColumn("dbo.Actors", "IsFavorite", c => c.Boolean(nullable: false));
            AddColumn("dbo.Actors", "Image", c => c.Binary());
        }
    }
}
