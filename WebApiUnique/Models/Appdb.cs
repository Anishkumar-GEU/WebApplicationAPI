using Microsoft.EntityFrameworkCore;
namespace WebApiUnique.Models
{
	public class Appdb : DbContext
	{
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;Database=db_WebApiUnique;User=root;Password=password",
            new MySqlServerVersion(new Version(9, 4, 0))
        );
        }

        public DbSet<Users> Users { get; set; }
    }
}

