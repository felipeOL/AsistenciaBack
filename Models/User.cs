using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AsistenciaBack.Model;

public class User : IdentityUser
{
	[JsonProperty("rut")]
	public string Rut { get; set; } = string.Empty;
	public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}