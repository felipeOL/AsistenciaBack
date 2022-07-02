using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AsistenciaBack.Model;

[Table("periodo")]
public class Period
{
	[JsonPropertyName("id"), Key, Required]
	public int Id { get; set; }
	[JsonPropertyName("nombre"), Required]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("anio"), Required]
	public int Year { get; set; }
	[JsonPropertyName("fechainicio"), Required]
	public DateTimeOffset Start { get; set; }
	[JsonPropertyName("fechafin"), Required]
	public DateTimeOffset End { get; set; }
}