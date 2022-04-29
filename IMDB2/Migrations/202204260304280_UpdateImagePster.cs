namespace IMDB2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImagePster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "Image", c => c.Binary());
            AddColumn("dbo.Movies", "Image", c => c.Binary());
            AddColumn("dbo.Directors", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Directors", "Image");
            DropColumn("dbo.Movies", "Image");
            DropColumn("dbo.Actors", "Image");
        }
    }
}
