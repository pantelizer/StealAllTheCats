using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StealAllTheCats.Models;
namespace StealAllTheCats
{
    public class StealCatsDbContext : DbContext
    {
        public StealCatsDbContext(DbContextOptions<StealCatsDbContext> options) : base(options)
        {

        }

        public DbSet<CatEntity> Cats { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<CatTag> CatTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatEntity>(entity =>
            {
                entity.Property(c => c.Id)
                .ValueGeneratedOnAdd();
                entity.HasMany(c => c.Tags)
                .WithMany(c => c.Cats)
                .UsingEntity<CatTag>(
                    l => l.HasOne<TagEntity>().WithMany().HasForeignKey(e => e.TagId),
                    r => r.HasOne<CatEntity>().WithMany().HasForeignKey(e => e.CatId));
            });


            modelBuilder.Entity<TagEntity>(entity =>
            {
                entity.Property(c => c.Id)
                .ValueGeneratedOnAdd();
                entity.HasIndex(c => c.Name)
                .IsUnique();
                entity.HasMany(c => c.Cats)
                .WithMany(c => c.Tags);

            });


        }
    }
}
