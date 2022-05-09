using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class CourseRequest
{
	[JsonPropertyName("id_profesor")]
	public string ProfessorId { get; set; } = string.Empty;
	[JsonPropertyName("código")]
	public string Code { get; set; } = string.Empty;
	[JsonPropertyName("nombre")]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("sección")]
	public string Section { get; set; } = string.Empty;
	[JsonPropertyName("semestre")]
	public string Semester { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public string Block { get; set; } = string.Empty;
}