using Microsoft.EntityFrameworkCore;
using SoftCast.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Conteudo> Conteudos { get; set; }
    public DbSet<Criador> Criadores { get; set; }
    public DbSet<ItemPlaylist> ItensPlaylist { get; set; }
}
