namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagemappingproduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProductMappingImages",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.ImageId })
                .ForeignKey("dbo.tblProducts", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.tblProductImages", t => t.ImageId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ImageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblProductMappingImages", "ImageId", "dbo.tblProductImages");
            DropForeignKey("dbo.tblProductMappingImages", "ProductId", "dbo.tblProducts");
            DropIndex("dbo.tblProductMappingImages", new[] { "ImageId" });
            DropIndex("dbo.tblProductMappingImages", new[] { "ProductId" });
            DropTable("dbo.tblProductMappingImages");
        }
    }
}
