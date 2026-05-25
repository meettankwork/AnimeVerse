using AnimeVerse.Model;
using Microsoft.EntityFrameworkCore;

namespace AnimeVerse.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CharactersModel>()
                .HasKey(c => c.CharacterId);

            modelBuilder.Entity<Abilities>()
                .HasOne(c => c.Character)
                .WithMany(c => c.Abilities)
                .HasForeignKey(c => c.CharacterId);

            modelBuilder.Entity<CharactersModel>()
                .HasOne(c => c.Anime)
                .WithMany(a => a.CharactersNames)
                .HasForeignKey(c => c.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);              

        }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<CharactersModel> Characters { get; set; }

        public DbSet<Abilities> Abilities { get; set; }
    }
}
