using Newtonsoft.Json;

namespace AsistenciaBack.Model.Request;

public class CourseRequest
{
	[JsonProperty("id_profesor")]
	public string ProfessorId { get; set; } = string.Empty;
	[JsonProperty("código")]
	public string Code { get; set; } = string.Empty;
	[JsonProperty("nombre")]
	public string Name { get; set; } = string.Empty;
	[JsonProperty("sección")]
	public string Section { get; set; } = string.Empty;
	[JsonProperty("semestre")]
	public string Semester { get; set; } = string.Empty;
	[JsonProperty("bloque")]
	public string Block { get; set; } = string.Empty;
}