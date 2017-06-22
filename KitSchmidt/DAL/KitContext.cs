using KitSchmidt.DAL.Models;
using System.Data.Entity;

namespace KitSchmidt.DAL
{
    public class KitContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
    }
}