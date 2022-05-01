using AsistenciaBack.Model;

namespace AsistenciaBack.Context;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}