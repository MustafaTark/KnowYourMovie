namespace IMDB2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFirst : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Actors", "Image");
            DropColumn("dbo.Movies", "Image");
            DropColumn("dbo.Directors", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Directors", "Image", c => c.String());
            AddColumn("dbo.Movies", "Image", c => c.String());
            AddColumn("dbo.Actors", "Image", c => c.String());
        }
    }
}
