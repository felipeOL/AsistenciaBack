using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AsistenciaBack.Model;

[Table("clase")]
public class Clazz
{
	[JsonPropertyName("id"), Key, Required]
	public int Id { get; set; }
	[JsonPropertyName("sala"), Required]
	public string Room { get; set; } = string.Empty;
	[JsonPropertyName("modalidad"), Required]
	public string Mode { get; set; } = string.Empty;
	[JsonPropertyName("bloque"), Required]
	public Block? Block { get; set; }
	[JsonPropertyName("fecha"), Required]
	public DateTimeOffset Date { get; set; }
	[JsonPropertyName("curso"), Required]
	public virtual Course? Course { get; set; }
	[JsonPropertyName("usuarios"), Required]
	public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
}