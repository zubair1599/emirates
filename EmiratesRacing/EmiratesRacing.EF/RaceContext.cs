using EmiratesRacing.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmiratesRacing.EF
{
    public class RaceContext : DbContext
    {

        public DbSet<Runner> Runners { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<WinningPrice> WinningPrizes { get; set; }

        public DbSet<Race> Races { get; set; }

        public DbSet<Horse>  Horses { get; set; }
        public DbSet<Jockey> Jockeys { get; set; }
        public DbSet<Link> Links { get; set; }

        public RaceContext()
            : base("defaultConnStr")
        {
    
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Race>().HasKey(m => m.RaceID);
            modelBuilder.Entity<Trainer>().HasKey(m => m.TrainerID);
            modelBuilder.Entity<WinningPrice>().HasKey(m => m.WinningPriceID);
            modelBuilder.Entity<Horse>().HasKey(m => m.HorseID);
            modelBuilder.Entity<Jockey>().HasKey(m => m.JockeyID);
            modelBuilder.Entity<Runner>().HasKey(m => m.RunnerID);
            modelBuilder.Entity<Link>().HasKey(m => m.LinkID);

            modelBuilder.Entity<Link>().Property(s => s.LinkID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Race>().Property(s => s.RaceID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Trainer>().Property(s => s.TrainerID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<WinningPrice>().Property(s => s.WinningPriceID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Horse>().Property(s => s.HorseID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Jockey>().Property(s => s.JockeyID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Runner>().Property(s => s.RunnerID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //one to zeer one
            modelBuilder.Entity<Link>().HasMany(m => m.Races).WithRequired(m => m.Link);



            
            modelBuilder.Entity<Race>().HasMany(m => m.Runners).WithRequired(m => m.Race);





            modelBuilder.Entity<Horse>().HasMany(m => m.Runners).WithRequired(m => m.Horse);
            modelBuilder.Entity<Trainer>().HasMany(m => m.Runners).WithRequired(m => m.Trainer);
            modelBuilder.Entity<Jockey>().HasMany(m => m.Runners).WithRequired(m => m.Jockey);
            modelBuilder.Entity<Race>().HasMany(m => m.WinningPrices).WithRequired(m => m.Race);



            base.OnModelCreating(modelBuilder);

            
        }


    }
}
