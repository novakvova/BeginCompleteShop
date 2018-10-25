namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtblProductDescriptionImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProductDescriptionImages",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 150),
                        ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.tblProducts", t => t.ProductId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblProductDescriptionImages", "ProductId", "dbo.tblProducts");
            DropIndex("dbo.tblProductDescriptionImages", new[] { "ProductId" });
            DropTable("dbo.tblProductDescriptionImages");
        }
    }
}
