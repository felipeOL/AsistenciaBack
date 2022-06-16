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
	public BlockResponse BlockResponse { get; set; }
	[JsonPropertyName("fecha")]
	public DateTimeOffset Date { get; set; }
	[JsonPropertyName("asistio")]
	public bool IsAttended { get; set; }
	[JsonPropertyName("curso")]
	public CourseResponse? Course { get; set; }
}