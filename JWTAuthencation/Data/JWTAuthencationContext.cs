using Microsoft.EntityFrameworkCore;

namespace JWTAuthencation.Data
{
    public class JWTAuthencationContext : DbContext
    {
        public JWTAuthencationContext(DbContextOptions<JWTAuthencationContext> options):base(options){}
        public DbSet<JWTAuthencation.Models.Users> Users { get; set; } = default!;
    }
}
