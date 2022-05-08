using Newtonsoft.Json;

namespace AsistenciaBack.Model.Response;

public class ClazzResponse
{
	[JsonProperty("sala")]
	public string Room { get; set; } = string.Empty;
	[JsonProperty("modalidad")]
	public string Mode { get; set; } = string.Empty;
	[JsonProperty("bloque")]
	public string Block { get; set; } = string.Empty;
}