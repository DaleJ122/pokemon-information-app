using Microsoft.EntityFrameworkCore;

namespace PokemonApi.Infrastructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) {}
    public DbSet<Pokemon> Pokemons => Set<Pokemon>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Pokemon>()
            .Property(p => p.Abilities)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

        mb.Entity<Pokemon>()
            .Property(p => p.Types)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
}