using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AsistenciaBack.Model;

[Table("clase")]
public class Clazz
{
	[JsonProperty("id"), Key, Required]
	public int Id { get; set; }
	[JsonProperty("sala"), Required]
	public string Room { get; set; } = string.Empty;
	[JsonProperty("modalidad"), Required]
	public string Mode { get; set; } = string.Empty;
	[JsonProperty("bloque"), Required]
	public string Block { get; set; } = string.Empty;
	public virtual Course? Course { get; set; }
}