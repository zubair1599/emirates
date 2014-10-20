namespace EmiratesRacing.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialized : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Horses",
                c => new
                    {
                        HorseID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.HorseID);
            
            CreateTable(
                "dbo.Runners",
                c => new
                    {
                        RunnerID = c.Int(nullable: false, identity: true),
                        Equipment = c.String(),
                        Name = c.String(),
                        Position = c.Int(nullable: false),
                        Drawn = c.Int(nullable: false),
                        OR = c.String(),
                        Margin = c.String(),
                        Jockey_JockeyID = c.Int(nullable: false),
                        Race_RaceID = c.Int(nullable: false),
                        Trainer_TrainerID = c.Int(nullable: false),
                        Horse_HorseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RunnerID)
                .ForeignKey("dbo.Jockeys", t => t.Jockey_JockeyID, cascadeDelete: true)
                .ForeignKey("dbo.Races", t => t.Race_RaceID, cascadeDelete: true)
                .ForeignKey("dbo.Trainers", t => t.Trainer_TrainerID, cascadeDelete: true)
                .ForeignKey("dbo.Horses", t => t.Horse_HorseID, cascadeDelete: true)
                .Index(t => t.Jockey_JockeyID)
                .Index(t => t.Race_RaceID)
                .Index(t => t.Trainer_TrainerID)
                .Index(t => t.Horse_HorseID);
            
            CreateTable(
                "dbo.Jockeys",
                c => new
                    {
                        JockeyID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        URL = c.String(),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.JockeyID);
            
            CreateTable(
                "dbo.Races",
                c => new
                    {
                        RaceID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Date = c.DateTime(nullable: false),
                        RaceCourse = c.String(),
                        Time = c.DateTime(nullable: false),
                        Type = c.String(),
                        Class = c.String(),
                        TrackLength = c.String(),
                        TrackType = c.String(),
                        WinningPrice = c.Long(nullable: false),
                        WinningCurrency = c.String(),
                        Weather = c.String(),
                        TrackCondition = c.String(),
                        RailPosition = c.String(),
                        SafetyLimit = c.String(),
                        RunningTime = c.String(),
                        RunningTimeType = c.String(),
                        Location = c.String(),
                        Notes = c.String(),
                        Link_LinkID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RaceID)
                .ForeignKey("dbo.Links", t => t.Link_LinkID, cascadeDelete: true)
                .Index(t => t.Link_LinkID);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkID = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.LinkID);
            
            CreateTable(
                "dbo.WinningPrices",
                c => new
                    {
                        WinningPriceID = c.Int(nullable: false, identity: true),
                        RaceId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Price = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.WinningPriceID)
                .ForeignKey("dbo.Races", t => t.RaceId, cascadeDelete: true)
                .Index(t => t.RaceId);
            
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        TrainerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.TrainerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Runners", "Horse_HorseID", "dbo.Horses");
            DropForeignKey("dbo.Runners", "Trainer_TrainerID", "dbo.Trainers");
            DropForeignKey("dbo.WinningPrices", "RaceId", "dbo.Races");
            DropForeignKey("dbo.Runners", "Race_RaceID", "dbo.Races");
            DropForeignKey("dbo.Races", "Link_LinkID", "dbo.Links");
            DropForeignKey("dbo.Runners", "Jockey_JockeyID", "dbo.Jockeys");
            DropIndex("dbo.WinningPrices", new[] { "RaceId" });
            DropIndex("dbo.Races", new[] { "Link_LinkID" });
            DropIndex("dbo.Runners", new[] { "Horse_HorseID" });
            DropIndex("dbo.Runners", new[] { "Trainer_TrainerID" });
            DropIndex("dbo.Runners", new[] { "Race_RaceID" });
            DropIndex("dbo.Runners", new[] { "Jockey_JockeyID" });
            DropTable("dbo.Trainers");
            DropTable("dbo.WinningPrices");
            DropTable("dbo.Links");
            DropTable("dbo.Races");
            DropTable("dbo.Jockeys");
            DropTable("dbo.Runners");
            DropTable("dbo.Horses");
        }
    }
}
