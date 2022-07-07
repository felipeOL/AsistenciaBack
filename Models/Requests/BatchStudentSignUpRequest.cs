using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class BatchStudentSignUpRequest
{
	[JsonPropertyName("idcurso")]
	public int CourseId { get; set; }
	[JsonPropertyName("estudiantes")]
	public ICollection<string> Students { get; set; } = new HashSet<string>();
}