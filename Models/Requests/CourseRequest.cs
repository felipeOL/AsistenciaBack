using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class CourseRequest
{
	[JsonPropertyName("id_profesor")]
	public string ProfessorId { get; set; } = string.Empty;
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
}