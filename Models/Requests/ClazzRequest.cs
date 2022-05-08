using Newtonsoft.Json;

namespace AsistenciaBack.Model.Request;

public class ClazzRequest
{
	[JsonProperty("id_curso")]
	public int CourseId { get; set; }
	[JsonProperty("sala")]
	public string Room { get; set; } = string.Empty;
	[JsonProperty("modalidad")]
	public string Mode { get; set; } = string.Empty;
	[JsonProperty("bloque")]
	public string Block { get; set; } = string.Empty;
}