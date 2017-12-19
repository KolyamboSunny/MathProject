namespace MathProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NgonId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Ngons");
            AlterColumn("dbo.Ngons", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Ngons", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Ngons");
            AlterColumn("dbo.Ngons", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Ngons", "Id");
        }
    }
}
