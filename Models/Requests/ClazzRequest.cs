using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class ClazzRequest
{
	[JsonPropertyName("idcurso")]
	public int CourseId { get; set; }
	[JsonPropertyName("sala")]
	public string Room { get; set; } = string.Empty;
	[JsonPropertyName("modalidad")]
	public string Mode { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public BlockRequest? BlockRequest { get; set; }
	[JsonPropertyName("fecha")]
	public DateTimeOffset Date { get; set; }
}