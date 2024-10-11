using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess
{
    public partial class ExamExplotionDBEntities : DbContext
    {
        public ExamExplotionDBEntities()
            : base("name=ExamExplotionDBEntities")
        {
        }

        public virtual DbSet<Accessory> Accessory { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<Guest> Guest { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<PurchasedAccessory> PurchasedAccessory { get; set; }
        public virtual DbSet<Report> Report { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>()
                .HasMany(e => e.PurchasedAccessory)
                .WithRequired(e => e.Accessory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Player)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.Player)
                .WithMany(e => e.Game)
                .Map(m => m.ToTable("PlayersByGame").MapLeftKey("gameId").MapRightKey("playerId"));

            modelBuilder.Entity<Player>()
                .HasMany(e => e.PurchasedAccessory)
                .WithRequired(e => e.Player)
                .HasForeignKey(e => e.playerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.Report)
                .WithRequired(e => e.Player)
                .HasForeignKey(e => e.idPlayerReported)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.Player1)
                .WithMany(e => e.Player2)
                .Map(m => m.ToTable("Friend").MapLeftKey("playerId1").MapRightKey("playerId2"));

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Game)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.hostPlayerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Game1)
                .WithOptional(e => e.Users1)
                .HasForeignKey(e => e.winnerPlayerId);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.Guest)
                .WithRequired(e => e.Users);

            modelBuilder.Entity<Users>()
                .HasOptional(e => e.Player)
                .WithRequired(e => e.Users);
        }
    }
}
