namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filterstableadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblFilterCategories",
                c => new
                    {
                        FilterNameId = c.Int(nullable: false),
                        FilterValueId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FilterNameId, t.FilterValueId, t.CategoryId })
                .ForeignKey("dbo.tblCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.tblFilterName", t => t.FilterNameId, cascadeDelete: true)
                .ForeignKey("dbo.tblFilterValue", t => t.FilterValueId, cascadeDelete: true)
                .Index(t => t.FilterNameId)
                .Index(t => t.FilterValueId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.tblFilterName",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 258),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblFilterValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 258),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblFilterNameGroups",
                c => new
                    {
                        FilterNameId = c.Int(nullable: false),
                        FilterValueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FilterNameId, t.FilterValueId })
                .ForeignKey("dbo.tblFilterName", t => t.FilterNameId, cascadeDelete: true)
                .ForeignKey("dbo.tblFilterValue", t => t.FilterValueId, cascadeDelete: true)
                .Index(t => t.FilterNameId)
                .Index(t => t.FilterValueId);
            
            CreateTable(
                "dbo.tblfilters",
                c => new
                    {
                        FilterNameId = c.Int(nullable: false),
                        FilterValueId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FilterNameId, t.FilterValueId, t.ProductId })
                .ForeignKey("dbo.tblFilterName", t => t.FilterNameId, cascadeDelete: true)
                .ForeignKey("dbo.tblFilterValue", t => t.FilterValueId, cascadeDelete: true)
                .ForeignKey("dbo.tblProducts", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.FilterNameId)
                .Index(t => t.FilterValueId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblfilters", "ProductId", "dbo.tblProducts");
            DropForeignKey("dbo.tblfilters", "FilterValueId", "dbo.tblFilterValue");
            DropForeignKey("dbo.tblfilters", "FilterNameId", "dbo.tblFilterName");
            DropForeignKey("dbo.tblFilterNameGroups", "FilterValueId", "dbo.tblFilterValue");
            DropForeignKey("dbo.tblFilterNameGroups", "FilterNameId", "dbo.tblFilterName");
            DropForeignKey("dbo.tblFilterCategories", "FilterValueId", "dbo.tblFilterValue");
            DropForeignKey("dbo.tblFilterCategories", "FilterNameId", "dbo.tblFilterName");
            DropForeignKey("dbo.tblFilterCategories", "CategoryId", "dbo.tblCategories");
            DropIndex("dbo.tblfilters", new[] { "ProductId" });
            DropIndex("dbo.tblfilters", new[] { "FilterValueId" });
            DropIndex("dbo.tblfilters", new[] { "FilterNameId" });
            DropIndex("dbo.tblFilterNameGroups", new[] { "FilterValueId" });
            DropIndex("dbo.tblFilterNameGroups", new[] { "FilterNameId" });
            DropIndex("dbo.tblFilterCategories", new[] { "CategoryId" });
            DropIndex("dbo.tblFilterCategories", new[] { "FilterValueId" });
            DropIndex("dbo.tblFilterCategories", new[] { "FilterNameId" });
            DropTable("dbo.tblfilters");
            DropTable("dbo.tblFilterNameGroups");
            DropTable("dbo.tblFilterValue");
            DropTable("dbo.tblFilterName");
            DropTable("dbo.tblFilterCategories");
        }
    }
}
