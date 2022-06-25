using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class CourseRequest
{
	[JsonPropertyName("idprofesor")]
	public string TeacherId { get; set; } = string.Empty;
	[JsonPropertyName("codigo")]
	public string Code { get; set; } = string.Empty;
	[JsonPropertyName("nombre")]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("seccion")]
	public string Section { get; set; } = string.Empty;
	[JsonPropertyName("semestre")]
	public string Semester { get; set; } = string.Empty;
	[JsonPropertyName("bloques")]
	public virtual ICollection<BlockRequest> BlockRequests { get; set; } = new HashSet<BlockRequest>();
	[JsonPropertyName("anio")]
	public DateTimeOffset Year { get; set; }
}