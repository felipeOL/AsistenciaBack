using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class BlockRequest
{
	[JsonPropertyName("id")]
	public int Id { get; set; }
	[JsonPropertyName("dia")]
	public string Day { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public string Time { get; set; } = string.Empty;
}