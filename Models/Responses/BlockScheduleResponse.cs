using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class BlockScheduleResponse
{
	[JsonPropertyName("nombrecurso")]
	public string CourseName { get; set; } = string.Empty;
	[JsonPropertyName("seccioncurso")]
	public string CourseSection { get; set; } = string.Empty;
	[JsonPropertyName("nombreprofesor")]
	public string TeacherName { get; set; } = string.Empty;
	[JsonPropertyName("emailprofesor")]
	public string TeacherEmail { get; set; } = string.Empty;
	[JsonPropertyName("dia")]
	public string Day { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public string Time { get; set; } = string.Empty;
}