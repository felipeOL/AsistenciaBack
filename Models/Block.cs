using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AsistenciaBack.Model;

[Table("bloque")]
public class Block
{
	[JsonPropertyName("id"), Key, Required]
	public int Id { get; set; }
	[JsonPropertyName("dia"), Required]
	public string Day { get; set; } = string.Empty;
	[JsonPropertyName("bloque"), Required]
	public string Time { get; set; } = string.Empty;
	[JsonPropertyName("cursos"), Required]
	public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
	[JsonPropertyName("clases"), Required]
	public virtual ICollection<Clazz> Clazzs { get; set; } = new HashSet<Clazz>();
}