using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class BlockScheduleResponse
{
	[JsonPropertyName("nombrecurso")]
	public string CourseName { get; set; } = string.Empty;
	[JsonPropertyName("seccioncurso")]
	public string CourseSection { get; set; } = string.Empty;
	[JsonPropertyName("dia")]
	public string Day { get; set; } = string.Empty;
	[JsonPropertyName("bloque")]
	public string Time { get; set; } = string.Empty;
}