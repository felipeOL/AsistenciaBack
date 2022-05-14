using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class ClazzResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }
	[JsonPropertyName("sala")]
	public string Room { get; set; } = string.Empty;
	[JsonPropertyName("modalidad")]
	public string Mode { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public string Block { get; set; } = string.Empty;
	[JsonPropertyName("fecha")]
	public DateTime Date { get; set; }
	[JsonPropertyName("curso")]
	public CourseResponse? Course { get; set; }
}