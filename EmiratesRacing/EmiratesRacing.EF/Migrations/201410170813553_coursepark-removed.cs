namespace EmiratesRacing.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courseparkremoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Races", "RaceCourse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Races", "RaceCourse", c => c.String());
        }
    }
}
