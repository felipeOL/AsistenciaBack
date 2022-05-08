using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AsistenciaBack.Model;

[Table("curso")]
public class Course
{
	[JsonProperty("id"), Key, Required]
	public int Id { get; set; }
	[JsonProperty("código"), Required]
	public string Code { get; set; } = string.Empty;
	[JsonProperty("nombre"), Required]
	public string Name { get; set; } = string.Empty;
	[JsonProperty("sección"), Required]
	public string Section { get; set; } = string.Empty;
	[JsonProperty("semestre"), Required]
	public string Semester { get; set; } = string.Empty;
	[JsonProperty("bloque"), Required]
	public string Block { get; set; } = string.Empty;
	[Required]
	public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
	[Required]
	public virtual ICollection<Clazz> Clazzs { get; set; } = new HashSet<Clazz>();
}