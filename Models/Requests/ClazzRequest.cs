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
	public string Block { get; set; } = string.Empty;
	[JsonPropertyName("fecha")]
	public DateTime Date { get; set; }
}