using Microsoft.EntityFrameworkCore;
using ProjetoooAPI.Models;

namespace ProjetoooAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource=app.db; Cache=Shared");
        }
    }
}