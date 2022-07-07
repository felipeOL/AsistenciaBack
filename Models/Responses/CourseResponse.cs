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
	[JsonPropertyName("idperiodo")]
	public int PeriodId { get; set; }
	[JsonPropertyName("bloques")]
	public virtual ICollection<BlockResponse> BlockResponses { get; set; } = new HashSet<BlockResponse>();
	[JsonPropertyName("anio")]
	public DateTimeOffset Year { get; set; }
	[JsonPropertyName("profesor")]
	public UserResponse? Teacher { get; set; }
}