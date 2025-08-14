using SistemaMatheus.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaMatheus.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
    }
}