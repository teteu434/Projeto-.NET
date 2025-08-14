using SistemaMatheus.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaMatheus.Data
{
    // Classe responsável pela comunicação com o banco de dados via EF Core
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        
        // Mapeamento das tabelas no banco
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
    }
}