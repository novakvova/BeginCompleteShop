namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateproductsimages : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tblProductImage", newName: "tblProductImages");
            CreateIndex("dbo.tblProductImages", "FileName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.tblProductImages", new[] { "FileName" });
            RenameTable(name: "dbo.tblProductImages", newName: "tblProductImage");
        }
    }
}
