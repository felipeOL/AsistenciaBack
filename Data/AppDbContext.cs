using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AsistenciaBack.Context;

public class AppDbContext : IdentityDbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}