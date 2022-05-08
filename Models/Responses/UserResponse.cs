using Newtonsoft.Json;

namespace AsistenciaBack.Model.Response;

public class UserResponse
{
	[JsonProperty("id")]
	public string Id { get; set; } = string.Empty;
	[JsonProperty("nombre")]
	public string UserName { get; set; } = string.Empty;
	[JsonProperty("rut")]
	public string Rut { get; set; } = string.Empty;
}