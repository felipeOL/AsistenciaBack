using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AsistenciaBack.Model;

[Table("curso")]
public class Course
{
	[JsonPropertyName("id"), Key, Required]
	public int Id { get; set; }
	[JsonPropertyName("codigo"), Required]
	public string Code { get; set; } = string.Empty;
	[JsonPropertyName("nombre"), Required]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("seccion"), Required]
	public string Section { get; set; } = string.Empty;
	[JsonPropertyName("semestre"), Required]
	public string Semester { get; set; } = string.Empty;
	[JsonPropertyName("bloques"), Required]
	public virtual ICollection<Block> Blocks { get; set; } = new HashSet<Block>();
	[JsonPropertyName("anio"), Required]
	public DateTimeOffset Year { get; set; }
	[JsonPropertyName("usuarios"), Required]
	public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
	[JsonPropertyName("clases"), Required]
	public virtual ICollection<Clazz> Clazzs { get; set; } = new HashSet<Clazz>();
	[JsonPropertyName("periodo"), Required]
	public virtual Period? Period { get; set; }
}