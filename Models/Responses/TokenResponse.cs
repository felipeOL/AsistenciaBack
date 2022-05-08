using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class TokenResponse
{
	[JsonPropertyName("roles")]
	public IList<string>? Roles { get; set; }
	[JsonPropertyName("Token")]
	public string Token { get; set; } = string.Empty;
}