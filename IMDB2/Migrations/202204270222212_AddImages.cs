namespace IMDB2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "Image", c => c.Binary());
            AddColumn("dbo.Movies", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Image");
            DropColumn("dbo.Actors", "Image");
        }
    }
}
