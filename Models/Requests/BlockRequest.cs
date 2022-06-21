using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class BlockRequest
{
	[JsonPropertyName("dia")]
	public string Day { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public string Time { get; set; } = string.Empty;
}