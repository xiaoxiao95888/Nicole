namespace Nicole.Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductType_Id", c => c.Guid());
            CreateIndex("dbo.Products", "ProductType_Id");
            AddForeignKey("dbo.Products", "ProductType_Id", "dbo.ProductTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductType_Id", "dbo.ProductTypes");
            DropIndex("dbo.Products", new[] { "ProductType_Id" });
            DropColumn("dbo.Products", "ProductType_Id");
        }
    }
}
