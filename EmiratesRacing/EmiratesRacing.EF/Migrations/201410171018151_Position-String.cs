namespace EmiratesRacing.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Runners", "Position", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Runners", "Position", c => c.Int(nullable: false));
        }
    }
}
