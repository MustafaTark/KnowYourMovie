namespace IMDB2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDirector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Directors", "FirstName", c => c.String());
            AddColumn("dbo.Directors", "LastName", c => c.String());
            DropColumn("dbo.Directors", "Name");
            DropColumn("dbo.Directors", "Img");
            DropColumn("dbo.Directors", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Directors", "Image", c => c.Binary());
            AddColumn("dbo.Directors", "Img", c => c.String());
            AddColumn("dbo.Directors", "Name", c => c.String());
            DropColumn("dbo.Directors", "LastName");
            DropColumn("dbo.Directors", "FirstName");
        }
    }
}
