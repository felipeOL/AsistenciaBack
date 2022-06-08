using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class AddStudentToCourse
{
	[JsonPropertyName("idcurso")]
	public int CourseId { get; set; }
	[JsonPropertyName("idestudiante")]
	public string StudentId { get; set; } = string.Empty;
}