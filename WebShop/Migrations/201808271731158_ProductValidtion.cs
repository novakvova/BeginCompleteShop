namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductValidtion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblProducts", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.tblProducts", "Description", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblProducts", "Description", c => c.String());
            AlterColumn("dbo.tblProducts", "Name", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
