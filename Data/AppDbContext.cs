using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AsistenciaBack.Context;

public class AppDbContext : IdentityDbContext<User>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	public DbSet<Curso> Cursos {get; set;}
	public DbSet<Clase> Clases {get; set;}

}