namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtabletblUserProfiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblUserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Image = c.String(),
                        Phone = c.String(),
                        DateOfBirth = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblUserProfiles", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.tblUserProfiles", new[] { "Id" });
            DropTable("dbo.tblUserProfiles");
        }
    }
}
