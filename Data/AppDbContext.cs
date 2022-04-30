using AsistenciaBack.Model;
using Microsoft.EntityFrameworkCore;

namespace AsistenciaBack.Context;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
	public DbSet<User> Users { get; set; }
}