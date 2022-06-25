using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AsistenciaBack.Context;

public class AppDbContext : IdentityDbContext<User>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	public DbSet<Block>? Blocks { get; set; }
	public DbSet<Course>? Courses { get; set; }
	public DbSet<Clazz>? Clazzs { get; set; }
	public DbSet<Period>? Periods { get; set; }
}