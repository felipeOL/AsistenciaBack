using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class CourseResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }
	[JsonPropertyName("codigo")]
	public string Code { get; set; } = string.Empty;
	[JsonPropertyName("nombre")]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("seccion")]
	public string Section { get; set; } = string.Empty;
	[JsonPropertyName("semestre")]
	public string Semester { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public string Block { get; set; } = string.Empty;
	[JsonPropertyName("anio")]
	public DateTimeOffset Year { get; set; }
	[JsonPropertyName("profesor")]
	public UserResponse? Professor { get; set; }
}