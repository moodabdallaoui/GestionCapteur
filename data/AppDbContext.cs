using Microsoft.EntityFrameworkCore;
using GestionCapteurs.Data.Model;


namespace GestionCapteurs.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Capteur> Capteurs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}