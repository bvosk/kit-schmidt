using KitSchmidt.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace KitSchmidt.DAL
{
    public class KitContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:kit-schmidt.database.windows.net,1433;Initial Catalog=kit_schmidt;Persist Security Info=False;User ID=bvosk;Password=Kitty1313!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public DbSet<Message> Messages { get; set; }
    } 
}