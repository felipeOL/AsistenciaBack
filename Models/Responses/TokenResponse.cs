using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class TokenResponse
{
	[JsonPropertyName("roles")]
	public IList<string>? Roles { get; set; }
	[JsonPropertyName("token")]
	public string Token { get; set; } = string.Empty;
}