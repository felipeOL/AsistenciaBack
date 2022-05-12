using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AsistenciaBack.Model;

public class User : IdentityUser
{
	[JsonPropertyName("rut"), Required]
	public string Rut { get; set; } = string.Empty;
	[JsonPropertyName("nombre"), Required]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("cursos"), Required]
	public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}