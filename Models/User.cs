using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AsistenciaBack.Model;

public class User : IdentityUser
{
	[JsonPropertyName("rut")]
	public string Rut { get; set; } = string.Empty;
	[JsonPropertyName("nombre")]
	public string Name { get; set; } = string.Empty;
	public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}