using Newtonsoft.Json;

namespace AsistenciaBack.Model.Response;

public class TokenResponse
{
	[JsonProperty("roles")]
	public IList<string>? Roles { get; set; }
	[JsonProperty("Token")]
	public string Token { get; set; } = string.Empty;
}